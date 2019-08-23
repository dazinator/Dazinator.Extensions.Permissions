using Dazinator.Extensions.Permissions.EFCore.EntityConfiguration;
using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Dazinator.Extensions.Permissions.Tests
{

    public class TestDbContext : DbContext
    {


        public TestDbContext(DbContextOptions<TestDbContext> options)
       : base(options)
        {
        }     

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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
