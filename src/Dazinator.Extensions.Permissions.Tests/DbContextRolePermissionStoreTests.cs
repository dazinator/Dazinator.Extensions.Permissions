using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dazinator.Extensions.Permissions.Tests
{
    public class DbContextRolePermissionStoreTests
    {
        [Fact]
        public async Task CanGetRolePermissions()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddDbContext<TestDbContext>((options) =>
            {
                options.UseInMemoryDatabase(nameof(DbContextRolePermissionStoreTests));
            });

            services.AddPermissions<DefaultAppPermission, DefaultAppPermissionType, DefaultAppPermissionSubject, DefaultApp>((builder) =>
            {
                builder
                 .AddDbContextDefaultPermissionStore<TestDbContext>()
                 .AddAttributeModelSeeder()
                 .SeedPermissionsFromType(typeof(SystemPermissions));

                builder.AddRolePermissions<int, DefaultRolePermission>((a) =>
                {
                    a.AddDbContextDefaultRolePermissionStore<TestDbContext>();
                });
            });

            var sp = services.BuildServiceProvider();

            // Seed the database with app permissions.
            using (var scope = sp.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<IAppPermissionsSeeder<DefaultAppPermission, DefaultAppPermissionSubject, DefaultAppPermissionType, DefaultApp>>();
                var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionStore<DefaultApp, DefaultAppPermission, DefaultAppPermissionSubject, DefaultAppPermissionType>>();
                await seeder.Seed(permissionService);
            }

            // Act
            using (var scope = sp.CreateScope())
            {
                var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionStore<DefaultApp, DefaultAppPermission, DefaultAppPermissionSubject, DefaultAppPermissionType>>();
                var perm = permissionService.FindPermission("System", SystemPermissions.Roles.ToString(), (int)PermissionTypes.Create);


                var dbContext = scope.ServiceProvider.GetRequiredService<TestDbContext>();
                var newRole = new Role();
                newRole.Permissions.Add(new DefaultRolePermission() { Permission = perm });
                dbContext.Roles.Add(newRole);
                dbContext.SaveChanges();

                var newRoleId = newRole.Id;

                var store = scope.ServiceProvider.GetRequiredService<IRolePermissionStore<int, DefaultRolePermission, DefaultAppPermission, DefaultAppPermissionType>>();
                //   userPermissionStore.AddUserPermission(1, perm);
                var userPerms = await store.GetPermissionsAsync(newRoleId);

                //var app = permissionService.GetApp("System");
                Assert.NotNull(userPerms);
                Assert.Single(userPerms);

            }

        }
    }

}
