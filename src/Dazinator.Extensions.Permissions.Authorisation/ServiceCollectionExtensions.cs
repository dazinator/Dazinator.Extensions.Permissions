using Dazinator.Extensions.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPermissionAuthorisationPolicyProvider(this IServiceCollection services, Func<IServiceProvider, IAuthorizationPolicyProvider> innerProviderFactory = null)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, PermissionsAuthorizationPolicyProvider>(sp =>
            {
                return new PermissionsAuthorizationPolicyProvider(innerProviderFactory?.Invoke(sp));
            });

            services.AddSingleton<PermissionPolicyNameProvider>();
            return services;
        }
    }
}
