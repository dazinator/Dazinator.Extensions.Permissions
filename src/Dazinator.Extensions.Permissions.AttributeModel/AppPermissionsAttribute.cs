using System;

namespace Dazinator.Extensions.Permissions.AttributeModel
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
