using Dazinator.Extensions.Permissions.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Dazinator.Extensions.Permissions
{

    public class AppPermissionsSeeder<TAppPermission, TAppPermissionSubject, TAppPermissionType, TApp> : IAppPermissionsSeeder<TAppPermission, TAppPermissionSubject, TAppPermissionType, TApp>
          where TAppPermissionType : IAppPermissionType
          where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
          where TAppPermissionSubject : IAppPermissionSubject<TAppPermission, TAppPermissionType>
          where TApp : IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>

    {
        private readonly IOptions<AppPermissionsSeederOptions> _seedOptions;

        protected IPermissionStore<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType> PermissionService { get; set; }
        public AppPermissionsSeeder(IOptions<AppPermissionsSeederOptions> seedOptions)
        {
            _seedOptions = seedOptions;
            // _permissionService = permissionService;
        }

        public async Task Seed(IPermissionStore<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType> permissionService)
        {
            PermissionService = permissionService;
            foreach (var item in _seedOptions.Value.AppPermissionTypes.ToArray())
            {
                // Each type we are inspecting looks like this:

                //[AppPermissions(AppCodes.PlatformAdmin)]
                //public enum PlatformAdminAppPermissions
                //{
                //    [ApplicablePermissionTypes(PermissionTypes.Create, PermissionTypes.Edit, PermissionTypes.Delete, PermissionTypes.View)]
                //    Tenant = 1,
                //    [ApplicablePermissionTypes(PermissionTypes.View)]
                //    TenantsList = 2,
                //    [ApplicablePermissionTypes(PermissionTypes.Edit)]
                //    ScpSettings = 2,
                //    [ApplicablePermissionTypes(PermissionTypes.Edit)]
                //    SmtpSettings = 3,
                //    [ApplicablePermissionTypes(PermissionTypes.Execute)]
                //    DataExport = 4,
                //}

                AppPermissionsAttribute appPermssionAttribute = GetAppPermissionsAttribute(item);

                // Ensure the app is defined
                SeedAppPermissions(appPermssionAttribute, item);

            }

            await permissionService.SaveChangesAsync();
        }

        private static AppPermissionsAttribute GetAppPermissionsAttribute(Type item)
        {
            var appPermissionsAttributes = item.GetCustomAttributes(typeof(AppPermissionsAttribute)).ToArray();
            var foundAttribute = appPermissionsAttributes.First();
            var appPermssionAttribute = ((AppPermissionsAttribute)foundAttribute);
            return appPermssionAttribute;
        }

        protected virtual void SeedAppPermissions(AppPermissionsAttribute appPermssionAttribute, Type permissionsEnumType)
        {
            var app = PermissionService.GetOrCreateApp(appPermssionAttribute.AppCode);
            OnAppSeeded(app);

            // Ensure each subject is defined
            var enumValues = Enum.GetValues(permissionsEnumType);
            foreach (var subjectId in enumValues)
            {
                var subjectName = Enum.GetName(permissionsEnumType, subjectId);
                var appPermissionSubject = UpdateOrCreateSubject(app, subjectName, (int)subjectId);
                OnPermissionSubjectSeeded(appPermissionSubject);

                // ensure each permission is defined for each subject.
                var permissionField = permissionsEnumType.GetField(subjectName);
                var permissionTypesAttributes = permissionField.GetCustomAttributes(false).OfType<PermissionTypesAttribute>().ToList();
                SeedAppSubjectPermissions(permissionField, app, appPermissionSubject, permissionTypesAttributes);

            }
        }

        protected virtual void OnAppSeeded(TApp app)
        {
        }

        protected virtual void OnPermissionSubjectSeeded(TAppPermissionSubject appPermissionSubject)
        {
        }

        protected virtual void SeedAppSubjectPermissions(FieldInfo permissionField, TApp app, TAppPermissionSubject appPermissionSubject, List<PermissionTypesAttribute> permissionTypesAttributes)
        {
            var deps = new List<Tuple<TAppPermission, Tuple<TApp, TAppPermissionSubject, TAppPermissionType>>>();

            foreach (var permissionTypesAttribute in permissionTypesAttributes)
            {
                TAppPermissionType parentPermissionType = default(TAppPermissionType);
                TApp parentApp = default(TApp);
                TAppPermissionSubject parentSubject = default(TAppPermissionSubject);
               
                if (permissionTypesAttribute.DependsOnPermissionTypeId != null)
                {
                    parentPermissionType = PermissionService.GetOrCreatePermissionType(permissionTypesAttribute.DependsOnPermissionTypeId.Value, null);

                    if (permissionTypesAttribute.DependsOn != null)
                    {
                        // get app code, and suject id
                        // find or create the app / code permissions related to this first
                        var typeofParentPermission = permissionTypesAttribute.DependsOn.GetType();

                        // var typeofParentPermission = permissionTypesAttribute.DependsOn.GetType()
                        var parentPermissionAttribute = GetAppPermissionsAttribute(typeofParentPermission);
                        if (parentPermissionAttribute == null)
                        {
                            throw new InvalidOperationException("Permission DependsOn property set to an object that doesn't have an `AppPermissions` attribute.");
                        }

                        parentApp = PermissionService.GetOrCreateApp(parentPermissionAttribute.AppCode);
                        var parentSubjectId = (int)permissionTypesAttribute.DependsOn;
                        parentSubject = UpdateOrCreateSubject(parentApp, null, parentSubjectId);

                    }
                }        

                var applicablePermissionTypes = permissionTypesAttribute.PermissionTypes;

                foreach (var allowedPermissionType in applicablePermissionTypes)
                {

                    var appPermission = SeedAppPermission(app, appPermissionSubject, allowedPermissionType);

                    // check for dependency
                    if (parentPermissionType != null)
                    {

                        if (parentApp != null)
                        {
                            var depIdentifier = new Tuple<TApp, TAppPermissionSubject, TAppPermissionType>(parentApp, parentSubject, parentPermissionType);
                            deps.Add(new Tuple<TAppPermission, Tuple<TApp, TAppPermissionSubject, TAppPermissionType>>(appPermission, depIdentifier));
                        }
                        else
                        {
                            // use current app code and subject id
                            var depIdentifier = new Tuple<TApp, TAppPermissionSubject, TAppPermissionType>(app, appPermissionSubject, parentPermissionType);
                            deps.Add(new Tuple<TAppPermission, Tuple<TApp, TAppPermissionSubject, TAppPermissionType>>(appPermission, depIdentifier));
                            //  TAppPermission parent =
                        }
                    }
                }
            }

            // establish dependencies

            foreach (var item in deps)
            {
                var dependency = GetOrCreatePermission(item.Item2.Item1, item.Item2.Item2, item.Item2.Item3);
                item.Item1.Parent = dependency;
                dependency.Children.Add(item.Item1);
            }

        }

        protected virtual TAppPermission SeedAppPermission(TApp app, TAppPermissionSubject appPermissionSubject, Tuple<int, string> permissionType)
        {
            var appPermission = GetOrCreatePermission(app, appPermissionSubject, permissionType);
            OnPermissionSeeded(appPermission);
            return appPermission;
        }

        protected virtual void OnPermissionSeeded(TAppPermission appPermission)
        {
        }

        private TAppPermission GetOrCreatePermission(TApp app, TAppPermissionSubject appPermissionSubject, Tuple<int, string> allowedPermissionType)
        {
            var permissionType = PermissionService.GetOrCreatePermissionType(allowedPermissionType.Item1, allowedPermissionType.Item2);
            return GetOrCreatePermission(app, appPermissionSubject, permissionType);
        }

        private TAppPermission GetOrCreatePermission(TApp app, TAppPermissionSubject appPermissionSubject, TAppPermissionType permissionType)
        {
            var result = PermissionService.GetOrCreatePermission(app, appPermissionSubject, permissionType);
            return result;
        }

        private TAppPermissionSubject UpdateOrCreateSubject(TApp app, string name, int enumValue)
        {
            var result = PermissionService.GetOrCreateAppSubject(app, name, enumValue);
            if (!string.IsNullOrWhiteSpace(name))
            {
                if (result.Name != name)
                {
                    // update name
                    result.Name = name;
                }
            }
            return result;
        }

    }
}
