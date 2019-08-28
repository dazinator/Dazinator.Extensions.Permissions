using Dazinator.Extensions.Permissions.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Dazinator.Extensions.Permissions
{
    public class PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp>
          where TAppPermissionType : IAppPermissionType
          where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
          where TAppPermissionSubject : IAppPermissionSubject<TAppPermission, TAppPermissionType>
          where TApp : IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>
    {
        private readonly IServiceCollection _services;

        public PermissionsRegistrationBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IServiceCollection Services => _services;

        private static IServiceCollection AddScopedForwardedTo<TForwardedFrom, TForwardedTo>(IServiceCollection services)
        where TForwardedTo : class, TForwardedFrom
        where TForwardedFrom : class
        {
            services.AddScoped<TForwardedFrom>((x) => x.GetRequiredService<TForwardedTo>());
            return services;
        }

        public PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> AddStore<TPermissionStore>()
        where TPermissionStore : class, IPermissionStore<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType>

        {
            Services.AddScoped<IPermissionStore<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType>, TPermissionStore>();
            return this;
        }

        /// <summary>
        /// Register the PermissionStore such that it will be resolved when <typeparamref name="TDerivedInterface"/> is requested. Also forwards request from <see cref="IPermissionStore<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType>"/> to <typeparamref name="TDerivedInterface"/>
        /// </summary>
        /// <typeparam name="TPermissionStore"></typeparam>
        /// <typeparam name="TForwardedInterface"></typeparam>
        /// <returns></returns>
        public PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> AddStore<TPermissionStore, TDerivedInterface>()
            where TPermissionStore : class, TDerivedInterface
            where TDerivedInterface : class, IPermissionStore<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType>

        {
            AddScopedForwardedTo<IPermissionStore<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType>, TDerivedInterface>(Services);
            Services.AddScoped<TDerivedInterface, TPermissionStore>();
            return this;
        }

        public PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> AddSeeder()
        {
            // todo: needs to have dbcontext injected
            Services.AddScoped<IAppPermissionsSeeder<TAppPermission, TAppPermissionSubject, TAppPermissionType, TApp>, AppPermissionsSeeder<TAppPermission, TAppPermissionSubject, TAppPermissionType, TApp>>();
            return this;
        }

        public PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> AddSeeder<TSeeder>()
             where TSeeder : AppPermissionsSeeder<TAppPermission, TAppPermissionSubject, TAppPermissionType, TApp>
        {
            // todo: needs to have dbcontext injected
            Services.AddScoped<TSeeder>();
            Services.AddScoped<IAppPermissionsSeeder<TAppPermission, TAppPermissionSubject, TAppPermissionType, TApp>, TSeeder>();
            return this;
        }

        public PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> SeedPermissionsFromType<TType>()
            where TType : struct
        {
            var permType = typeof(TType);
            Services.Configure<AppPermissionsSeederOptions>((a) =>
            {
                a.AppPermissionTypes.Add(permType);
            });
            return this;
        }

        // extension method for ef core.
    }
}
