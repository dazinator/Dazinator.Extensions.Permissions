using Dazinator.Extensions.Permissions;
using Dazinator.Extensions.Permissions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
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
        public static PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> AddDbContextPermissionService<TDbContext, TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp>(this PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> builder)
          where TDbContext : DbContext
          where TAppPermissionType : class, IAppPermissionType, new()
          where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
          where TAppPermissionSubject : class, IAppPermissionSubject<TAppPermission, TAppPermissionType>, new()
          where TApp : class,IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>, new()
           
        {
            builder.AddStore<DbContextPermissionStore<TDbContext, TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType>>();
            return builder;
        }


        /// <summary>
        /// Adds an <see cref="IPermissionStore{TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType}"/> backed by a DbContext using a standard set of default entities. Don't use this if you need to customise the entities. 
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static PermissionsRegistrationBuilder<DefaultAppPermission, DefaultAppPermissionType, DefaultAppPermissionSubject, DefaultApp> AddDbContextDefaultPermissionService<TDbContext>(this PermissionsRegistrationBuilder<DefaultAppPermission, DefaultAppPermissionType, DefaultAppPermissionSubject, DefaultApp> builder)
           where TDbContext : DbContext
        {
            builder.AddStore<DefaultDbContextPermissionStore<TDbContext>>();
            return builder;
        }

    }
}
