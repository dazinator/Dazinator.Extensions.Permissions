Extend an ASP.NET Core application with a permissions model.


## Define some permissions structure

1. Add nuget package `Dazinator.Extensions.Permissions`

2. Create an enum with some attributes to define your permissions structure. Note each set of permissions has an `AppCode` in this case the permissions below are defined for "MyCoolApp".
This is designed for modular systems, where different extensions / modules can introduce their own sets of permissions. The `AppCode` is the way to indicate which Module / Extension the permissions are relevent for.
If your application does not host different Modules / Extensions within it, then feel free to just use a single code for all your permissions sets - i.e "System"

```
using Dazinator.Extensions.Permissions;

namespace Foo
{
    [AppPermissions("MyCoolApp")]
    public enum MyCoolAppPermissions
    {        
        [ApplicablePermissionTypes(PermissionTypes.Create, PermissionTypes.Edit, PermissionTypes.Delete, PermissionTypes.View)]
        BlogPost = 1,
        [ApplicablePermissionTypes(PermissionTypes.View)]
        WeatherForecast = 2,
        [ApplicablePermissionTypes(PermissionTypes.Edit)]
        Settings = 3,        
        [ApplicablePermissionTypes(PermissionTypes.Execute)]
        ExportBlogs = 4,
    }
}
```

Each permission consists of a permission type e.g `Create` etc and a `Subject` e.g 'BlogPost'
The above therefore describes the following permissions that will be available in the application in total:

1. Create BlogPost
2. Edit BlogPost
3. Delete BlogPost
4. View BlogPost
5. View WeatherForecast
6. Edit Settings
7. Execute ExportBlogs

## Create the data model

1. Add nuget package `Dazinator.Extensions.Permissions.EFCore`

2. In your EF Core `DbContext` class, in the `OnModelCreating` method, apply the following entity configurations. 
Note, if you need to customise the permissions data model, you need to create your own custom entities rather than using the Default entity types. You can also derive your own IEntityConfigurations.


```csharp
            var appConfig = new DefaultAppConfiguration<DefaultApp, DefaultAppPermissionType, DefaultAppPermission, DefaultAppPermissionSubject>();
            builder.ApplyConfiguration(appConfig);

            var appPermissionConfig = new DefaultAppPermissionConfiguration<DefaultAppPermission, DefaultAppPermissionType>();
            builder.ApplyConfiguration(appPermissionConfig);

            var appPermissionSubjectConfig = new DefaultAppPermissionSubjectConfiguration<DefaultAppPermissionSubject, DefaultAppPermissionType, DefaultAppPermission>();
            builder.ApplyConfiguration(appPermissionSubjectConfig);

            var appPermissionTypeConfig = new DefaultAppPermissionTypeConfiguration<DefaultAppPermissionType>();
            builder.ApplyConfiguration(appPermissionTypeConfig);
```

## Register the IPermissionService which can be used to get / create permissions.

```
            // permission pased authorisation policy provider
            services.AddPermissions<AppPermission, AppPermissionType, AppPermissionSubject, App>((builder) =>
            {
                builder.AddDbContextPermissionService<YourDbContext, AppPermission, AppPermissionType, AppPermissionSubject, App>()
            });
```

You can now inject `IPermissionsService` anywhere you need to get or create new app permissions.

## Seed database with permissions

To seed the database with the permissions you have defined via enum and attribute usage:

```
            services.AddPermissions<AppPermission, AppPermissionType, AppPermissionSubject, App>((builder) =>
            {
                builder.AddDbContextPermissionService<YourDbContext, AppPermission, AppPermissionType, AppPermissionSubject, App>()
                       .AddSeeder()
					   .SeedPermissionsFromType<MyCoolAppPermissions>();
			});
           
```

You can now inject / resolve the `IAppPermissionsSeeder` somewhere on startup and use it like so:

```
var seeder = sp.GetRequiredService<IAppPermissionsSeeder<AppPermission, AppPermissionSubject, AppPermissionType, App>>();
await seeder.Seed();
```

If you look in the database, the tables will now be populated with apps and permissions, matching your enum / attribute definition.


## Authorisation based on Permissions


1. Add nuget package `Dazinator.Extensions.Permissions.Authorisation`

In startup.cs


```

            services.AddPermissions<AppPermission, AppPermissionType, AppPermissionSubject, App>((builder) =>
            {
                builder.AddAuthorisationPolicyProvider()            
                // rest omitted for brevity
            });

```

This registers a custom AuthorisationPolicyProvider for permissions wirth asp.net core authorisation system.

2. Above your controllers you can use the following attribute:

```

        [PermissionAuthorizeAttribute("MyCoolApp", MyCoolAppPermissions.WeatherForecast, PermissionTypes.View)]
        [HttpGet("[action]")]
        public IActionResult WeatherForecast()
        {           
            return Ok(); // return weather forecast here - it's ok as user has View WeatherForecast permission
        }

```

Note: if you get sick and tired of repeating the "App Code" i.e "MyCoolApp" when you can derive your own attribute:


```

    public class MyCoolAppPermissionAuthorizeAttribute : PermissionAuthorizeAttribute
    {      
        /// <summary>
        /// Creates a new instance of <see cref="AuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="permissions">A list of permissions to authorize</param>
        public MyCoolAppPermissionAuthorizeAttribute(MyCoolAppPermissions subject, params PermissionTypes[] permissionTypes):base("MyCoolApp", (int)subject, permissionTypes)
        {
            
        }
    }
```

Now you can just do this:


```

        [MyCoolAppPermissionAuthorize(MyCoolAppPermissions.WeatherForecast, PermissionTypes.View)]
        [HttpGet("[action]")]
        public IActionResult WeatherForecast()
        {           
            return Ok(); // return weather forecast here - it's ok as user has View WeatherForecast permission
        }

```