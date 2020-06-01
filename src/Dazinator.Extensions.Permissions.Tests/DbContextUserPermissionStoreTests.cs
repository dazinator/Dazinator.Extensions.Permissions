using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dazinator.Extensions.Permissions.Tests
{
    public class DbContextUserPermissionStoreTests
    {
        [Fact]
        public async Task CanGetUserPermissions()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddDbContext<TestDbContext>((options) =>
            {
                options.UseInMemoryDatabase(nameof(DbContextUserPermissionStoreTests));
            });

            services.AddPermissions<DefaultAppPermission, DefaultAppPermissionType, DefaultAppPermissionSubject, DefaultApp>((builder) =>
            {
                builder
                 .AddDbContextDefaultPermissionStore<TestDbContext>()
                 .AddAttributeModelSeeder()
                 .SeedPermissionsFromType(typeof(SystemPermissions));

                builder.AddUserPermissions<int, DefaultUserPermission>((a) =>
                {
                    a.AddDbContextDefaultUserPermissionStore<TestDbContext>();
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
                var newUser = new User();
                newUser.Permissions.Add(new DefaultUserPermission() { Permission = perm });
                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();

                var newUserId = newUser.Id;

                var userPermissionStore = scope.ServiceProvider.GetRequiredService<IUserPermissionStore<int, DefaultUserPermission, DefaultAppPermission, DefaultAppPermissionType>>();
                //   userPermissionStore.AddUserPermission(1, perm);
                var userPerms = await userPermissionStore.GetPermissionsAsync(newUserId);

                //var app = permissionService.GetApp("System");
                Assert.NotNull(userPerms);
                Assert.Single(userPerms);

            }




        }
    }

}
