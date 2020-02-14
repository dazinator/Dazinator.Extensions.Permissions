using Dazinator.Extensions.Permissions.Authorisation;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dazinator.Extensions.Permissions
{
    public class PermissionsAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        public const string PolicyPrefix = "PERM:";
        private readonly IAuthorizationPolicyProvider _innerProvider;
        private readonly Task<AuthorizationPolicy> NullResult = Task.FromResult(default(AuthorizationPolicy));

        public PermissionsAuthorizationPolicyProvider(IAuthorizationPolicyProvider innerProvider = null)
        {
            _innerProvider = innerProvider;
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            if (_innerProvider == null)
            {
                return NullResult;
            }
            return _innerProvider?.GetDefaultPolicyAsync();
        }

#if NETSTANDARD2_1
        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            if (_innerProvider == null)
            {
                return NullResult;
            }
            return _innerProvider?.GetFallbackPolicyAsync();
        }
#endif

        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith(PermissionsAuthorizationPolicyProvider.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                if (_innerProvider == null)
                {
                    return NullResult;
                }
                return _innerProvider?.GetPolicyAsync(policyName);
            }

            // TODO: Cache?
            var permissionSegments = policyName.Substring(PermissionsAuthorizationPolicyProvider.PolicyPrefix.Length).Split(':');
            var appCode = permissionSegments[0];
            var subjectId = permissionSegments[1];
            var permissionTypes = permissionSegments[2].Split(',');

            var permissionClaimValues = new List<string>(permissionTypes.Length);
            foreach (var permissionType in permissionTypes)
            {
                var permission = $"{appCode}-{subjectId}-{permissionType}";
                permissionClaimValues.Add(permission);
            }

            var policy = new AuthorizationPolicyBuilder()
                .RequireClaim(CustomClaimTypes.Permission, permissionClaimValues)
                .Build();

            return Task.FromResult(policy);
        }
    }
}
