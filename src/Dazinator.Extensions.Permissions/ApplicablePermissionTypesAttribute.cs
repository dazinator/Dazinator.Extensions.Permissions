using System;

namespace Dazinator.Extensions.Permissions
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ApplicablePermissionTypesAttribute : Attribute
    {
        public ApplicablePermissionTypesAttribute(params PermissionTypes[] types)
        {
            ApplicablePermissionTypes = types;
        }

        public PermissionTypes[] ApplicablePermissionTypes { get; }

    }


}
