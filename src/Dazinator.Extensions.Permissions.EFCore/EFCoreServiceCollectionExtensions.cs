using Dazinator.Extensions.Permissions;
using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EFCoreServiceCollectionExtensions
    {

        /// <summary>
        /// Adds an <see cref="IPermissionStore{TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType}"/> backed by a DbContext using the specified entity types. Use this if you have derived your own entities. 
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <typeparam name="TAppPermission"></typeparam>
        /// <typeparam name="TAppPermissionType"></typeparam>
        /// <typeparam name="TAppPermissionSubject"></typeparam>
        /// <typeparam name="TApp"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> AddDbContextPermissionStore<TDbContext, TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp>(this PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> builder)
          where TDbContext : DbContext
          where TAppPermissionType : class, IAppPermissionType, new()
          where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
          where TAppPermissionSubject : class, IAppPermissionSubject<TAppPermission, TAppPermissionType>, new()
          where TApp : class, IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>, new()

        {
            builder.AddPermissionStore<DbContextPermissionStore<TDbContext, TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType>>();
            return builder;
        }


        /// <summary>
        /// Adds an <see cref="IPermissionStore{TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType}"/> backed by a DbContext using a standard set of default entities. Don't use this if you need to customise the entities. 
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static PermissionsRegistrationBuilder<DefaultAppPermission, DefaultAppPermissionType, DefaultAppPermissionSubject, DefaultApp> AddDbContextDefaultPermissionStore<TDbContext>(this PermissionsRegistrationBuilder<DefaultAppPermission, DefaultAppPermissionType, DefaultAppPermissionSubject, DefaultApp> builder)
           where TDbContext : DbContext
        {
            builder.AddPermissionStore<DefaultDbContextPermissionStore<TDbContext>>();
            return builder;
        }

        public static UserPermissionsRegistrationBuilder<int, DefaultUserPermission, DefaultAppPermission, DefaultAppPermissionType> AddDbContextDefaultUserPermissionStore<TDbContext>(this UserPermissionsRegistrationBuilder<int, DefaultUserPermission, DefaultAppPermission, DefaultAppPermissionType> builder)
           where TDbContext : DbContext
        {
            builder.AddUserPermissionStore<DefaultDbContextUserPermissionStore<TDbContext>>();
            return builder;
        }

        public static RolePermissionsRegistrationBuilder<int, DefaultRolePermission, DefaultAppPermission, DefaultAppPermissionType> AddDbContextDefaultRolePermissionStore<TDbContext>(this RolePermissionsRegistrationBuilder<int, DefaultRolePermission, DefaultAppPermission, DefaultAppPermissionType> builder)
           where TDbContext : DbContext
        {
            builder.AddRolePermissionStore<DefaultDbContextRolePermissionStore<TDbContext>>();
            return builder;
        }




    }
}
