using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Dazinator.Extensions.Permissions
{
    public class DbContextRolePermissionStore<TContext, TRoleIdKey, TRolePermission, TAppPermission, TAppPermissionType> : IRolePermissionStore<TRoleIdKey, TRolePermission, TAppPermission, TAppPermissionType>
      where TContext : DbContext
      where TRoleIdKey : IEquatable<TRoleIdKey>
      where TRolePermission : class, IRolePermission<TRoleIdKey, TAppPermission, TAppPermissionType>, new()
      where TAppPermissionType : IAppPermissionType
      where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
    {        
        /// <summary>
        /// Creates a new instance of the store.
        /// </summary>
        /// <param name="context">The context used to access the store.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/> used to describe store errors.</param>
        public DbContextRolePermissionStore(TContext context)
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

        private DbSet<TRolePermission> RolePermissions { get { return Context.Set<TRolePermission>(); } }

        private DbSet<TAppPermission> AppPermissions { get { return Context.Set<TAppPermission>(); } }


        /// <summary>
        /// Retrieves the permissions for the role with the specified <paramref name="roleId"/>.
        /// </summary>
        /// <param name="roleId">The id for the user whose permissions should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the permissions the user is a member of.</returns>        public override async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        public async Task<IList<TAppPermission>> GetPermissionsAsync(TRoleIdKey roleId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            var query = from userPermission in RolePermissions
                        join permission in AppPermissions.Include(a=>a.Parent) on userPermission.PermissionId equals permission.Id
                        where userPermission.RoleId.Equals(roleId)
                        select permission;

            return await query.ToListAsync(cancellationToken);
        }

        public void AddRolePermission(TRoleIdKey roleId, TAppPermission permission)
        {
            if (permission == null)
            {
                throw new ArgumentNullException(nameof(permission));
            }
            RolePermissions.Add(CreateRolePermission(roleId, permission));
        }

        public void RemoveRolePermission(TRoleIdKey roleId, TAppPermission permission)
        {
            if (permission == null)
            {
                throw new ArgumentNullException(nameof(permission));
            }
            RolePermissions.Remove(CreateRolePermission(roleId, permission));
        }
        
        protected virtual TRolePermission CreateRolePermission(TRoleIdKey roleId, TAppPermission permission)
        {
            var perm = new TRolePermission() { RoleId = roleId, Permission = permission };
            return perm;
        }

        public Task<bool> HasPermissionAsync(TRoleIdKey roleId, int permissionId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return RolePermissions.AnyAsync(a => a.RoleId.Equals(roleId) && a.PermissionId == permissionId);  
        }
        
        public Task SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }       
    }

}