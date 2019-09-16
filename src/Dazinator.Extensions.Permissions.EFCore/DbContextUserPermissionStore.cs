using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Dazinator.Extensions.Permissions
{
    public class DbContextUserPermissionStore<TContext, TUserIdKey, TUserPermission, TAppPermission, TAppPermissionType> : IUserPermissionStore<TUserIdKey, TUserPermission, TAppPermission, TAppPermissionType>
      where TContext : DbContext
      where TUserIdKey : IEquatable<TUserIdKey>
      where TUserPermission : class, IUserPermission<TUserIdKey, TAppPermission, TAppPermissionType>, new()
      where TAppPermissionType : IAppPermissionType
      where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
    {        
        /// <summary>
        /// Creates a new instance of the store.
        /// </summary>
        /// <param name="context">The context used to access the store.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/> used to describe store errors.</param>
        public DbContextUserPermissionStore(TContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            Context = context;
        }

        /// <summary>
        /// Gets the database context for this store.
        /// </summary>
        public virtual TContext Context { get; private set; }

        private DbSet<TUserPermission> UserPermissions { get { return Context.Set<TUserPermission>(); } }

        private DbSet<TAppPermission> AppPermissions { get { return Context.Set<TAppPermission>(); } }
                     

        /// <summary>
        /// Retrieves the permissions for the user with the specified <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The id for the user whose permissions should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the permissions the user is a member of.</returns>        public override async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        public async Task<IList<TAppPermission>> GetPermissionsAsync(TUserIdKey userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = from userPermission in UserPermissions
                        join permission in AppPermissions.Include(a=>a.Parent) on userPermission.PermissionId equals permission.Id
                        where userPermission.UserId.Equals(userId)
                        select permission;

            return await query.ToListAsync(cancellationToken);
        }

        public void AddUserPermission(TUserIdKey userId, TAppPermission permission)
        {
            if (permission == null)
            {
                throw new ArgumentNullException(nameof(permission));
            }
            UserPermissions.Add(CreateUserPermission(userId, permission));
        }

        public void RemoveUserPermission(TUserIdKey userId, TAppPermission permission)
        {
            if (permission == null)
            {
                throw new ArgumentNullException(nameof(permission));
            }
            UserPermissions.Remove(CreateUserPermission(userId, permission));
        }
        
        protected virtual TUserPermission CreateUserPermission(TUserIdKey userId, TAppPermission permission)
        {
            var perm = new TUserPermission() { UserId = userId, Permission = permission };
            return perm;
        }

        public Task<bool> HasPermissionAsync(TUserIdKey userId, int permissionId, CancellationToken cancellationToken = default)
        {
            return UserPermissions.AnyAsync(a => a.UserId.Equals(userId) && a.PermissionId == permissionId);  
        }

        ///// <summary>
        ///// Return a permission with the specified app id, subject id, and permission type id if it exists.
        ///// </summary>
        ///// <param name="appId">The id of the app that the permission belongs under.</param>
        ///// <param name="subjectId">The id of the subject for which the permission governs.</param>
        ///// <param name="typeId">The id of the permission type./param>
        ///// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        ///// <returns>The role if it exists.</returns>
        //protected Task<TAppPermission> FindPermissionAsync(int permissionId, CancellationToken cancellationToken)
        //{
        //    return this.AppPermissions.FindAsync(new object[] { permissionId }, cancellationToken);
        //}       


        public Task SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }       
    }

}