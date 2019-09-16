using Dazinator.Extensions.Permissions.EFCore.EntityConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Dazinator.Extensions.Permissions.Tests
{


    public class TestDbContext : DbContext
    {
        
        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options)
       : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().HasMany(a => a.Permissions).WithOne().HasForeignKey(a => a.UserId);
            builder.Entity<Role>().HasMany(a => a.Permissions).WithOne().HasForeignKey(a => a.RoleId);


            builder.ApplyConfiguration(new DefaultAppConfiguration());
            builder.ApplyConfiguration(new DefaultAppPermissionConfiguration());
            builder.ApplyConfiguration(new DefaultAppPermissionSubjectConfiguration());
            builder.ApplyConfiguration(new DefaultAppPermissionTypeConfiguration());
            //This will singularize all table names
            // ef core 2.2
            foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
            {
                entityType.Relational().TableName = entityType.DisplayName();
            }

            ////  ef core 3.0.0
            //foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
            //{
            //    entityType.SetTableName(entityType.DisplayName());
            //}


        }

    }

}
