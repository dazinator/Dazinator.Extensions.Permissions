using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dazinator.Extensions.Permissions
{
    public class DefaultDbContextRolePermissionStore<TDbContext> : DbContextRolePermissionStore<TDbContext, int, DefaultRolePermission, DefaultAppPermission, DefaultAppPermissionType>
      where TDbContext : DbContext
    {
        public DefaultDbContextRolePermissionStore(TDbContext dbContext) : base(dbContext)
        {
        }
    }

}