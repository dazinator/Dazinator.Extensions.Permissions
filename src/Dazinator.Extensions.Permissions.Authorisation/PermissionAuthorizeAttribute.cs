using Microsoft.AspNetCore.Authorization;

namespace Dazinator.Extensions.Permissions
{
    public class PermissionAuthorizeAttribute : AuthorizeAttribute
    {
        internal const string PolicyPrefix = "PERM:";
        private readonly string _appCode;
        private readonly int _subjectId;

        /// <summary>
        /// Creates a new instance of <see cref="AuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="permissions">A list of permissions to authorize</param>
        public PermissionAuthorizeAttribute(string AppCode, int subjectId, params PermissionTypes[] permissionTypes)
        {
            Policy = $"{PolicyPrefix}{_appCode}:{_subjectId}:{string.Join(",", permissionTypes)}";
            _appCode = AppCode;
            _subjectId = subjectId;
        }
     
    }
}