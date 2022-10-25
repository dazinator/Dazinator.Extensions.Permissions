using Dazinator.Extensions.Permissions.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dazinator.Extensions.Permissions
{
    //fyi https://github.com/aspnet/AspNetCore/blob/839cf8925278018903f53f22d580d15b0a59ca0f/src/Identity/EntityFrameworkCore/src/UserStore.cs
    public interface IUserPermissionStore<TUserIdKey, TUserPermission, TAppPermission, TAppPermissionType>
        where TUserIdKey : IEquatable<TUserIdKey>
        where TUserPermission : IUserPermission<TUserIdKey, TAppPermission, TAppPermissionType>
        where TAppPermissionType : IAppPermissionType
        where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
    {

        /// <summary>
        /// Retrieves the permissions for the user with the specified <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The id for the user whose permissions should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the permissions the user is a member of.</returns>
        Task<IList<TAppPermission>> GetPermissionsAsync(TUserIdKey userId, CancellationToken cancellationToken = default(CancellationToken));

        void AddUserPermission(TUserIdKey userId, TAppPermission permission);

        //void AddUserPermission(TUserIdKey userId, int permissionId);

        void RemoveUserPermission(TUserIdKey userId, TAppPermission permission);

        /// <summary>
        /// Returns a flag indicating if the specified user has a permission assigned to them.
        /// </summary>
        Task<bool> HasPermissionAsync(TUserIdKey userId, int permissionId, CancellationToken cancellationToken = default(CancellationToken));

        Task SaveChangesAsync();
    }

}