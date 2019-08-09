using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dazinator.Extensions.Permissions
{
    public class DefaultDbContextPermissionService<TDbContext> : DbContextPermissionService<TDbContext, DefaultApp, DefaultAppPermission, DefaultAppPermissionSubject, DefaultAppPermissionType>
      where TDbContext : DbContext
    {
        public DefaultDbContextPermissionService(TDbContext dbContext) : base(dbContext)
        {
        }
    }
}