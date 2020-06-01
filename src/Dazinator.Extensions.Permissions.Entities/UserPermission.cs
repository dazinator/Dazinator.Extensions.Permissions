using System;

namespace Dazinator.Extensions.Permissions.Entities
{
    /// <summary>
    /// Represents the link between a user and a permission.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key used for users.</typeparam>
    public abstract class UserPermission<TKey, TAppPermission, TAppPermissionType>: IUserPermission<TKey, TAppPermission, TAppPermissionType>
        where TKey : IEquatable<TKey>
        where TAppPermissionType : IAppPermissionType
        where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
    {
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the primary key of the user that is linked to a permission.
        /// </summary>
        public virtual TKey UserId { get; set; }

        /// <summary>
        /// Gets or sets the primary key of the permission that is linked to the user.
        /// </summary>
        public virtual int PermissionId { get; set; }

        public virtual TAppPermission Permission { get; set; }
    }
}
