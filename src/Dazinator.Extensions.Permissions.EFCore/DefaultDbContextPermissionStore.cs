using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dazinator.Extensions.Permissions
{
    public class DefaultDbContextPermissionStore<TDbContext> : DbContextPermissionStore<TDbContext, DefaultApp, DefaultAppPermission, DefaultAppPermissionSubject, DefaultAppPermissionType>
      where TDbContext : DbContext
    {
        public DefaultDbContextPermissionStore(TDbContext dbContext) : base(dbContext)
        {
        }
    }

}