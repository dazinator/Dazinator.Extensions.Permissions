using Dazinator.Extensions.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPermissionAuthorisationPolicyProvider(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionsAuthorizationPolicyProvider>();
            services.AddSingleton<PermissionPolicyNameProvider>();
            return services;
        }     
    }
}
