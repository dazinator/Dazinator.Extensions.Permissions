using Dazinator.Extensions.Permissions.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Dazinator.Extensions.Permissions
{
    public class UserPermissionsRegistrationBuilder<TUserKey, TUserPermission, TAppPermission, TAppPermissionType>
      where TAppPermissionType : IAppPermissionType
      where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
      where TUserKey : IEquatable<TUserKey>
      where TUserPermission : IUserPermission<TUserKey, TAppPermission, TAppPermissionType>
    {
        private readonly IServiceCollection _services;

        public UserPermissionsRegistrationBuilder(IServiceCollection services)
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

        public UserPermissionsRegistrationBuilder<TUserKey, TUserPermission, TAppPermission, TAppPermissionType> AddUserPermissionStore<TUserPermissionStore>()
        where TUserPermissionStore : class, IUserPermissionStore<TUserKey, TUserPermission,  TAppPermission, TAppPermissionType>
        {
            Services.AddScoped<IUserPermissionStore<TUserKey, TUserPermission, TAppPermission, TAppPermissionType>, TUserPermissionStore>();
            return this;
        }

      
        public UserPermissionsRegistrationBuilder<TUserKey, TUserPermission, TAppPermission, TAppPermissionType> AddUserPermissionsStore<TUserPermissionStore, TDerivedInterface>()
            where TUserPermissionStore : class, TDerivedInterface
            where TDerivedInterface : class, IUserPermissionStore<TUserKey, TUserPermission, TAppPermission, TAppPermissionType>

        {
            AddScopedForwardedTo<IUserPermissionStore<TUserKey, TUserPermission, TAppPermission, TAppPermissionType>, TDerivedInterface>(Services);
            Services.AddScoped<TDerivedInterface, TUserPermissionStore>();
            return this;
        }

    }

}
