using Dazinator.Extensions.Permissions.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dazinator.Extensions.Permissions
{
    public class RolePermissionsRegistrationBuilder<TRoleKey, TRolePermission, TAppPermission, TAppPermissionType>
  where TAppPermissionType : IAppPermissionType
  where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
  where TRoleKey : IEquatable<TRoleKey>
  where TRolePermission : IRolePermission<TRoleKey, TAppPermission, TAppPermissionType>
    {
        private readonly IServiceCollection _services;

        public RolePermissionsRegistrationBuilder(IServiceCollection services)
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

        public RolePermissionsRegistrationBuilder<TRoleKey, TRolePermission, TAppPermission, TAppPermissionType> AddRolePermissionStore<TRolePermissionStore>()
        where TRolePermissionStore : class, IRolePermissionStore<TRoleKey, TRolePermission, TAppPermission, TAppPermissionType>
        {
            Services.AddScoped<IRolePermissionStore<TRoleKey, TRolePermission, TAppPermission, TAppPermissionType>, TRolePermissionStore>();
            return this;
        }


        public RolePermissionsRegistrationBuilder<TRoleKey, TRolePermission, TAppPermission, TAppPermissionType> AddUserPermissionsStore<IRolePermissionStore, TDerivedInterface>()
            where IRolePermissionStore : class, TDerivedInterface
            where TDerivedInterface : class, IRolePermissionStore<TRoleKey, TRolePermission, TAppPermission, TAppPermissionType>

        {
            AddScopedForwardedTo<IRolePermissionStore<TRoleKey, TRolePermission, TAppPermission, TAppPermissionType>, TDerivedInterface>(Services);
            Services.AddScoped<TDerivedInterface, IRolePermissionStore>();
            return this;
        }

    }

}
