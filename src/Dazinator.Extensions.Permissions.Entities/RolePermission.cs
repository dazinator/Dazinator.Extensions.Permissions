using System;

namespace Dazinator.Extensions.Permissions.Entities
{
    /// <summary>
    /// Represents the link between a role and a permission.
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key used for roles.</typeparam>
    public abstract class RolePermission<TKey, TAppPermission, TAppPermissionType>: IRolePermission<TKey, TAppPermission, TAppPermissionType>
        where TKey : IEquatable<TKey>
        where TAppPermissionType : IAppPermissionType
        where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
    {
        public virtual int Id { get; set; }

        /// <summary>
        /// Gets or sets the primary key of the role that is linked to a permission.
        /// </summary>
        public virtual TKey RoleId { get; set; }

        /// <summary>
        /// Gets or sets the primary key of the permission that is linked to the role.
        /// </summary>
        public virtual int PermissionId { get; set; }

        public virtual TAppPermission Permission { get; set; }
    }
}
