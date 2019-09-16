using Dazinator.Extensions.Permissions.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dazinator.Extensions.Permissions
{
    public interface IRolePermissionStore<TRoleIdKey, TRolePermission, TAppPermission, TAppPermissionType>
        where TRoleIdKey : IEquatable<TRoleIdKey>
        where TRolePermission : IRolePermission<TRoleIdKey, TAppPermission, TAppPermissionType>
        where TAppPermissionType : IAppPermissionType
        where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
    {

        /// <summary>
        /// Retrieves the permissions for the role with the specified <paramref name="roleId"/>.
        /// </summary>
        /// <param name="roleId">The id for the role whose permissions should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the permissions the role is a member of.</returns>
        Task<IList<TAppPermission>> GetPermissionsAsync(TRoleIdKey roleId, CancellationToken cancellationToken = default(CancellationToken));

        void AddRolePermission(TRoleIdKey roleId, TAppPermission permission);

        //void AddUserPermission(TUserIdKey userId, int permissionId);

        void RemoveRolePermission(TRoleIdKey roleId, TAppPermission permission);

        /// <summary>
        /// Returns a flag indicating if the specified role has a permission assigned.
        /// </summary>
        Task<bool> HasPermissionAsync(TRoleIdKey roleId, int permissionId, CancellationToken cancellationToken = default(CancellationToken));

        Task SaveChangesAsync();
    }

}