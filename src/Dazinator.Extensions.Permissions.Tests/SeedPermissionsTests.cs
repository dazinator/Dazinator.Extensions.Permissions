using Dazinator.Extensions.Permissions.AttributeModel;
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
                options.UseInMemoryDatabase(nameof(DbContextPermissionStoreTests));
            });

            services.AddPermissions<DefaultAppPermission, DefaultAppPermissionType, DefaultAppPermissionSubject, DefaultApp>((builder) =>
            {
                builder
                 .AddDbContextDefaultPermissionStore<TestDbContext>()
                 .AddAttributeModelSeeder()
                 .SeedPermissionsFromType(typeof(SystemPermissions))
                 .SeedPermissionsFromType(typeof(AddOnPermissions));                
            });

            var sp = services.BuildServiceProvider();

            // Act
            // Seed the database with app permissions.
            using (var scope = sp.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<IAppPermissionsSeeder<DefaultAppPermission, DefaultAppPermissionSubject, DefaultAppPermissionType, DefaultApp>>();
                var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionStore<DefaultApp, DefaultAppPermission, DefaultAppPermissionSubject, DefaultAppPermissionType>>();
                await seeder.Seed(permissionService);
            }

            // assert


            // Assert
            using (var scope = sp.CreateScope())
            {
                var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionStore<DefaultApp, DefaultAppPermission, DefaultAppPermissionSubject, DefaultAppPermissionType>>();
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

        [Fact]
        public async Task CanSeedPermissionsWithDependenciesComplex()
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
                 .AddDbContextDefaultPermissionStore<TestDbContext>()
                 .AddAttributeModelSeeder()
                 .SeedPermissionsFromType(typeof(ComplexPermissions));               
            });

            var sp = services.BuildServiceProvider();

            // Act
            // Seed the database with app permissions.
            using (var scope = sp.CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<IAppPermissionsSeeder<DefaultAppPermission, DefaultAppPermissionSubject, DefaultAppPermissionType, DefaultApp>>();
                var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionStore<DefaultApp, DefaultAppPermission, DefaultAppPermissionSubject, DefaultAppPermissionType>>();
                await seeder.Seed(permissionService);
            }

            // assert


            // Assert
            using (var scope = sp.CreateScope())
            {
                var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionStore<DefaultApp, DefaultAppPermission, DefaultAppPermissionSubject, DefaultAppPermissionType>>();
                var app = permissionService.GetApp("CX");
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

        [AppPermissions("CX")]
        public enum ComplexPermissions
        {
            [PermissionTypes(PermissionTypes.Edit, DependsOnPermissionType = PermissionTypes.View)]
            [PermissionTypes(PermissionTypes.View, PermissionTypes.Create, PermissionTypes.Delete,
                DependsOn = Batman)]
            Foo = 1,
            [PermissionTypes(PermissionTypes.View,
                DependsOn = Batman)]
            Bar = 2,
            [PermissionTypes(PermissionTypes.Edit, DependsOnPermissionType = PermissionTypes.View)]
            [PermissionTypes(PermissionTypes.View,
                DependsOn = Batman)]
            Baz = 3,
            [PermissionTypes(PermissionTypes.Edit,
                DependsOn = Batman)]
            Bat = 4,
            [PermissionTypes(PermissionTypes.Execute,
                DependsOn = Batman)]
            Bam = 5,
            [PermissionTypes(CustomPermissionTypes.Foo)]
            Batman = 6,
        }

        public enum CustomPermissionTypes
        {
            Foo = 10
        }

    }

}
