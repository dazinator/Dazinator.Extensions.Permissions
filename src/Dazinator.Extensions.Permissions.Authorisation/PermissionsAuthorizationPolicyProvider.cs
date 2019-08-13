using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dazinator.Extensions.Permissions
{
    public partial class PermissionsAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionsAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options)
        {
        }

        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (!policyName.StartsWith(PermissionAuthorizeAttribute.PolicyPrefix, StringComparison.OrdinalIgnoreCase))
            {
                return base.GetPolicyAsync(policyName);
            }

            var permissionSegments = policyName.Substring(PermissionAuthorizeAttribute.PolicyPrefix.Length).Split(':');
            var appCode = permissionSegments[0];
            var subjectId = permissionSegments[1];
            var permissionTypes = permissionSegments[2].Split(',');

            var permissionClaimValues = new List<string>(permissionTypes.Length);
            foreach (var permissionType in permissionTypes)
            {
                var permission = $"{appCode}-{subjectId}-{permissionType}";
                permissionClaimValues.Add(permission);
            }
           
            // Policy = $"{PolicyPrefix}{_appCode}:{_subject}:{string.Join(",", permissionTypes)}";

            // var permissionNames = policyName.Substring(PermissionAuthorizeAttribute.PolicyPrefix.Length).Split(',');

            var policy = new AuthorizationPolicyBuilder()
                .RequireClaim(CustomClaimTypes.Permission, permissionClaimValues)
                .Build();

            return Task.FromResult(policy);
        }
    }
}
