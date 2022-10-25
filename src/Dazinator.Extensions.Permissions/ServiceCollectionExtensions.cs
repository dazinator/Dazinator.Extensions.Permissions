using Dazinator.Extensions.Permissions;
using Dazinator.Extensions.Permissions.Entities;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPermissions<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp>(this IServiceCollection services, Action<PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp>> configure)
          where TAppPermissionType : IAppPermissionType
          where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
          where TAppPermissionSubject : IAppPermissionSubject<TAppPermission, TAppPermissionType>
          where TApp : IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>
        {
            var builder = new PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp>(services);
            configure(builder);
            return services;
        }

    }
}
