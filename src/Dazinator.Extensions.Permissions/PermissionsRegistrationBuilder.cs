using Dazinator.Extensions.Permissions.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dazinator.Extensions.Permissions
{
    public class PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp>
          where TAppPermissionType : IAppPermissionType
          where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
          where TAppPermissionSubject : IAppPermissionSubject<TAppPermission, TAppPermissionType>
          where TApp : IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>
    {
        private readonly IServiceCollection _services;

        public PermissionsRegistrationBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IServiceCollection Services => _services;

        private static IServiceCollection AddScopedForwardedTo<TForwardedFrom, TForwardedTo>(IServiceCollection services)
        where TForwardedTo : class, TForwardedFrom
        where TForwardedFrom : class
        {
            services.AddScoped<TForwardedFrom>((x) => x.GetRequiredService<TForwardedTo>());
            return services;
        }

        public PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> AddPermissionStore<TPermissionStore>()
        where TPermissionStore : class, IPermissionStore<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType>

        {
            Services.AddScoped<IPermissionStore<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType>, TPermissionStore>();
            return this;
        }

        /// <summary>
        /// Register the PermissionStore such that it will be resolved when <typeparamref name="TDerivedInterface"/> is requested. Also forwards request from <see cref="IPermissionStore<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType>"/> to <typeparamref name="TDerivedInterface"/>
        /// </summary>
        /// <typeparam name="TPermissionStore"></typeparam>
        /// <typeparam name="TForwardedInterface"></typeparam>
        /// <returns></returns>
        public PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> AddPermissionStore<TPermissionStore, TDerivedInterface>()
            where TPermissionStore : class, TDerivedInterface
            where TDerivedInterface : class, IPermissionStore<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType>

        {
            AddScopedForwardedTo<IPermissionStore<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType>, TDerivedInterface>(Services);
            Services.AddScoped<TDerivedInterface, TPermissionStore>();
            return this;
        }

        public PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> AddUserPermissions<TUserIdKey, TUserPermission>(Action<UserPermissionsRegistrationBuilder<TUserIdKey, TUserPermission, TAppPermission, TAppPermissionType>> configure)
            where TUserIdKey : IEquatable<TUserIdKey>
            where TUserPermission : IUserPermission<TUserIdKey, TAppPermission, TAppPermissionType>
        {
            var builder = new UserPermissionsRegistrationBuilder<TUserIdKey, TUserPermission, TAppPermission, TAppPermissionType>(Services);
            configure(builder);
            return this;
        }

        public PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> AddRolePermissions<TRoleIdKey, TRolePermission>(Action<RolePermissionsRegistrationBuilder<TRoleIdKey, TRolePermission, TAppPermission, TAppPermissionType>> configure)
           where TRoleIdKey : IEquatable<TRoleIdKey>
           where TRolePermission : IRolePermission<TRoleIdKey, TAppPermission, TAppPermissionType>
        {
            var builder = new RolePermissionsRegistrationBuilder<TRoleIdKey, TRolePermission, TAppPermission, TAppPermissionType>(Services);
            configure(builder);
            return this;
        }

    }
}
