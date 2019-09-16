using System;

namespace Dazinator.Extensions.Permissions.Entities
{
    public interface IUserPermission<TKey, TAppPermission, TAppPermissionType>
         where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the primary key of the user that is linked to a permission.
        /// </summary>
        TKey UserId { get; set; }

        /// <summary>
        /// Gets or sets the primary key of the permission that is linked to the user.
        /// </summary>
        int PermissionId { get; set; }

        /// <summary>
        /// Gets or sets the permission that is linked to the user.
        /// </summary>
        TAppPermission Permission { get; set; }
    }

}
