using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Dazinator.Extensions.Permissions.Tests
{
    public class SeedPermissionsTests
    {

        [Fact]
        public async Task CanSeedPermissionsWithDependencies()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddDbContext<TestDbContext>((options) =>
            {
                options.UseInMemoryDatabase(nameof(DbContextPermissionServiceTests));
            });

            services.AddPermissions<DefaultAppPermission, DefaultAppPermissionType, DefaultAppPermissionSubject, DefaultApp>((builder) =>
            {
                builder
                 .AddDbContextDefaultPermissionService<TestDbContext>()
                .AddSeeder()
                .SeedPermissionsFromType<SystemPermissions>()
                .SeedPermissionsFromType<AddOnPermissions>();
            });

            var sp = services.BuildServiceProvider();

            // Act
            // Seed the database with app permissions.
            using (var scope = sp.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<IAppPermissionsSeeder<DefaultAppPermission, DefaultAppPermissionSubject, DefaultAppPermissionType, DefaultApp>>();
                var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService<DefaultApp, DefaultAppPermission, DefaultAppPermissionSubject, DefaultAppPermissionType>>();
                await seeder.Seed(permissionService);
            }

            // assert


            // Assert
            using (var scope = sp.CreateScope())
            {
                var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService<DefaultApp, DefaultAppPermission, DefaultAppPermissionSubject, DefaultAppPermissionType>>();
                var app = permissionService.GetApp("AddOn");
                Assert.NotNull(app);

                var perms = app.Permissions;
                Assert.NotNull(perms);

                var subjects = app.Subjects;
                Assert.NotNull(subjects);

                var permissionSubjects = Enum.GetNames(typeof(AddOnPermissions));
                Assert.NotNull(subjects);
                Assert.Equal(permissionSubjects.Count(), permissionSubjects.Count());

                var perm = perms.First();
                Assert.NotNull(perm.ParentId);

                // edit depends on view

              //  var editPerm = perms.Skip(1).Take(1).First();

                Assert.NotNull(perm.ParentId);

                // var parent = perm.Parent;
                //   var children = perm.Children;


                //var parentPerm = permissionService.FindPermission("System", "Users", PermissionTypes.View);
                //var children = parentPerm.Children;
            }




        }

    }

}
