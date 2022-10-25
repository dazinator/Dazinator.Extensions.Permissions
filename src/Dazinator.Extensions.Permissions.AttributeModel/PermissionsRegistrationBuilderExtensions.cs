using Dazinator.Extensions.Permissions.AttributeModel;
using Dazinator.Extensions.Permissions.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dazinator.Extensions.Permissions
{
    public static class PermissionsRegistrationBuilderExtensions
    {

        /// <summary>
        /// Adds an <see cref="AppPermissionsSeeder{TAppPermission, TAppPermissionSubject, TAppPermissionType,TApp}"/> that can populate a store with permissions defined via attributes on an enum.
        /// </summary>
        /// <typeparam name="TAppPermission"></typeparam>
        /// <typeparam name="TAppPermissionType"></typeparam>
        /// <typeparam name="TAppPermissionSubject"></typeparam>
        /// <typeparam name="TApp"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> AddAttributeModelSeeder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp>(this PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> builder)
          where TAppPermissionType : class, IAppPermissionType, new()
          where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
          where TAppPermissionSubject : class, IAppPermissionSubject<TAppPermission, TAppPermissionType>, new()
          where TApp : class, IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>, new()

        {
            builder.Services.AddScoped<IAppPermissionsSeeder<TAppPermission, TAppPermissionSubject, TAppPermissionType, TApp>, AppPermissionsSeeder<TAppPermission, TAppPermissionSubject, TAppPermissionType, TApp>>();
            return builder;
        }

        public static PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> AddAttributeModelSeeder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp, TSeeder>(this PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> builder)
  where TAppPermissionType : class, IAppPermissionType, new()
  where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
  where TAppPermissionSubject : class, IAppPermissionSubject<TAppPermission, TAppPermissionType>, new()
  where TApp : class, IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>, new()
  where TSeeder : AppPermissionsSeeder<TAppPermission, TAppPermissionSubject, TAppPermissionType, TApp>
        {
            builder.Services.AddScoped<TSeeder>();
            builder.Services.AddScoped<IAppPermissionsSeeder<TAppPermission, TAppPermissionSubject, TAppPermissionType, TApp>, AppPermissionsSeeder<TAppPermission, TAppPermissionSubject, TAppPermissionType, TApp>>();
            return builder;
        }

        public static PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> SeedPermissionsFromType<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp>(this PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> builder, Type type)
where TAppPermissionType : class, IAppPermissionType, new()
where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
where TAppPermissionSubject : class, IAppPermissionSubject<TAppPermission, TAppPermissionType>, new()
where TApp : class, IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>, new()
        {
            var permType = type;
            builder.Services.Configure<AppPermissionsSeederOptions>((a) =>
            {
                a.AppPermissionTypes.Add(permType);
            });
            return builder;
        }
    }
}
