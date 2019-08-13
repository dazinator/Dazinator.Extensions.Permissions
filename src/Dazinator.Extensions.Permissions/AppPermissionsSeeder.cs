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
          where TAppPermission : IAppPermission<TAppPermissionType>
          where TAppPermissionSubject : IAppPermissionSubject<TAppPermission, TAppPermissionType>
          where TApp : IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>

    {
        private readonly IOptions<AppPermissionsSeederOptions> _seedOptions;

        protected IPermissionService<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType> PermissionService { get; set; }
        public AppPermissionsSeeder(IOptions<AppPermissionsSeederOptions> seedOptions)
        {
            _seedOptions = seedOptions;
            // _permissionService = permissionService;
        }

        public async Task Seed(IPermissionService<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType> permissionService)
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

                var appPermissionsAttributes = item.GetCustomAttributes(typeof(AppPermissionsAttribute)).ToArray();
                var foundAttribute = appPermissionsAttributes.First();
                var appPermssionAttribute = ((AppPermissionsAttribute)foundAttribute);

                // Ensure the app is defined
                SeedAppPermissions(appPermssionAttribute, item);

            }

            await permissionService.SaveChangesAsync();
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
                var appPermissionSubject = GetOrCreateSubject(app, subjectName, (int)subjectId);
                OnPermissionSubjectSeeded(appPermissionSubject);

                // ensure each permission is defined for each subject.
                var permissionField = permissionsEnumType.GetField(subjectName);
                var permissionTypesAttributes = permissionField.GetCustomAttributes(false).OfType<ApplicablePermissionTypesAttribute>().ToList();
                SeedAppSubjectPermissions(permissionField, app, appPermissionSubject, permissionTypesAttributes);

            }
        }

        protected virtual void OnAppSeeded(TApp app)
        {
        }

        protected virtual void OnPermissionSubjectSeeded(TAppPermissionSubject appPermissionSubject)
        {
        }

        protected virtual void SeedAppSubjectPermissions(FieldInfo permissionField, TApp app, TAppPermissionSubject appPermissionSubject, List<ApplicablePermissionTypesAttribute> permissionTypesAttributes)
        {
            foreach (var permissionTypesAttribute in permissionTypesAttributes)
            {
                var applicablePermissionTypes = permissionTypesAttribute.ApplicablePermissionTypes;

                foreach (var allowedPermissionType in applicablePermissionTypes)
                {
                    SeedAppPermission(app, appPermissionSubject, allowedPermissionType);
                }
            }

        }

        protected virtual void SeedAppPermission(TApp app, TAppPermissionSubject appPermissionSubject, PermissionTypes allowedPermissionType)
        {
            var appPermission = GetOrCreatePermission(app, appPermissionSubject, allowedPermissionType);
            OnPermissionSeeded(appPermission);
        }

        protected virtual void OnPermissionSeeded(TAppPermission appPermission)
        {
        }

        private TAppPermission GetOrCreatePermission(TApp app, TAppPermissionSubject appPermissionSubject, PermissionTypes allowedPermissionType)
        {
            return PermissionService.GetOrCreatePermission(app, appPermissionSubject, allowedPermissionType);
        }

        private TAppPermissionSubject GetOrCreateSubject(TApp app, string name, int enumValue)
        {
            return PermissionService.GetOrCreateAppSubject(app, name, enumValue);
        }

    }
}
