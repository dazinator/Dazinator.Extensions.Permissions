using Dazinator.Extensions.Permissions;
using Dazinator.Extensions.Permissions.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Adds an <see cref="IPermissionStore{TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType}"/> backed by a DbContext using the specified entity types. Use this if you have derived your own entities. 
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TAppPermission"></typeparam>
        /// <typeparam name="TAppPermissionType"></typeparam>
        /// <typeparam name="TAppPermissionSubject"></typeparam>
        /// <typeparam name="TApp"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> AddAuthorisationPolicyProvider<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp>(this PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> builder)
          where TAppPermissionType : IAppPermissionType, new()
          where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
          where TAppPermissionSubject : IAppPermissionSubject<TAppPermission, TAppPermissionType>, new()
          where TApp : IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>, new()

        {
            builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionsAuthorizationPolicyProvider>();
            return builder;
        }     

    }
}
