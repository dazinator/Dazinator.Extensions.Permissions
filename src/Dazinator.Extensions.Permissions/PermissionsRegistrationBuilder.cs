using Dazinator.Extensions.Permissions.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Dazinator.Extensions.Permissions
{
    public class PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp>
          where TAppPermissionType : IAppPermissionType
          where TAppPermission : IAppPermission<TAppPermissionType>
          where TAppPermissionSubject : IAppPermissionSubject<TAppPermission, TAppPermissionType>
          where TApp : IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>
    {
        private readonly IServiceCollection _services;

        public PermissionsRegistrationBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public IServiceCollection Services => _services;

        public PermissionsRegistrationBuilder<TAppPermission, TAppPermissionType, TAppPermissionSubject, TApp> AddPermissionService<TPermissionService>()
        where TPermissionService : class, IPermissionService<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType>

        {
            Services.AddScoped<IPermissionService<TApp, TAppPermission, TAppPermissionSubject, TAppPermissionType>, TPermissionService>();
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
