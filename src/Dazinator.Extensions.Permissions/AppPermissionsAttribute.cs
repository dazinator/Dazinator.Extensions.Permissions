using System;

namespace Dazinator.Extensions.Permissions
{
    public class AppPermissionsAttribute : Attribute
    {
        public AppPermissionsAttribute(string appCode)
        {
            AppCode = appCode;
        }

        public string AppCode { get; }
    }
}
