using Dazinator.Extensions.Permissions;

namespace Microsoft.Extensions.DependencyInjection
{
    public class PermissionPolicyNameProvider
    {
        public string GetPolicyName(string appCode, int subjectId, int[] permissionTypes)
        {
            var permissionTypesToString = string.Join(",", permissionTypes);
            return $"{PermissionsAuthorizationPolicyProvider.PolicyPrefix}{appCode}:{subjectId}:{permissionTypesToString}";
        }

        public string GetPolicyName(string appCode, int subjectId, int permissionType)
        {
            return $"{PermissionsAuthorizationPolicyProvider.PolicyPrefix}{appCode}:{subjectId}:{permissionType}";
        }
    }
}
