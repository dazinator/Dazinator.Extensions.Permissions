using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dazinator.Extensions.Permissions
{
    public class DefaultDbContextUserPermissionStore<TDbContext> : DbContextUserPermissionStore<TDbContext,int, DefaultUserPermission, DefaultAppPermission, DefaultAppPermissionType>
      where TDbContext : DbContext
    {
        public DefaultDbContextUserPermissionStore(TDbContext dbContext) : base(dbContext)
        {
        }
    }
}