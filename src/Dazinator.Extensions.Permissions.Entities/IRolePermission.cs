using System;

namespace Dazinator.Extensions.Permissions.Entities
{
    public interface IRolePermission<TKey, TAppPermission, TAppPermissionType>
     where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the primary key of the role that is linked to a permission.
        /// </summary>
        TKey RoleId { get; set; }

        /// <summary>
        /// Gets or sets the primary key of the permission that is linked to the role.
        /// </summary>
        int PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the permission.
        /// </summary>
        TAppPermission Permission { get; set; }
    }

}
