using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dazinator.Extensions.Permissions.Tests
{
    public class DbContextPermissionStoreTests
    {

        [Fact]
        public async Task CanGetAppWithPermissions()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddDbContext<TestDbContext>((options) =>
            {
                options.UseInMemoryDatabase(nameof(DbContextPermissionStoreTests));
            });

            services.AddPermissions<DefaultAppPermission, DefaultAppPermissionType, DefaultAppPermissionSubject, DefaultApp>((builder) =>
            {
                builder
                 .AddDbContextDefaultPermissionService<TestDbContext>()
                 .AddAttributeModelSeeder()
                 .SeedPermissionsFromType(typeof(SystemPermissions));
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
                var app = permissionService.GetApp("System");
                Assert.NotNull(app);

                var perms = app.Permissions;
                Assert.NotNull(perms);

                var subjects = app.Subjects;
                Assert.NotNull(subjects);

                var permissionSubjects = Enum.GetNames(typeof(SystemPermissions));
                Assert.NotNull(subjects);
                Assert.Equal(permissionSubjects.Count(), permissionSubjects.Count());

            }




        }

    }

}
