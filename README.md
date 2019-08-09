Extend an ASP.NET Core application with a permissions model.


## Define some permissions structure

1. Add nuget package `Dazinator.Extensions.Permissions`

2. Create an enum with some attributes to define your permissions structure. Note each set of permissions has an `AppCode` in this case the permissions below are defined for "MyCoolApp".
This is designed for modular systems, where different extensions / modules can introduce their own sets of permissions. The `AppCode` is the way to indicate which Module / Extension the permissions are relevent for.
If your application does not host different Modules / Extensions within it, then feel free to just use a single code for all your permissions sets - i.e "System"

```
using Dazinator.Extensions.Permissions;

namespace Hub.Platform.Server.Services.Authorisation
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



In your dbcontext:

```
TODO
```

## Authorise based on these Permissions

1. Add nuget package `Dazinator.Extensions.Permissions.Authorisation`

In startup.cs


```

            services.AddPermissions<AppPermission, AppPermissionType, AppPermissionSubject, App>((builder) =>
            {
                builder.AddAuthorisationPolicyProvider()            
                // omitted for brevity
            });

```

2. Above your controllers:

```

        [PermissionAuthorizeAttribute("MyCoolApp", MyCoolAppPermissions.WeatherForecast, PermissionTypes.View)]
        [HttpGet("[action]")]
        public IActionResult WeatherForecast()
        {           
            return Ok(); // return weather forecast here - it's ok as user has View WeatherForecast permission
        }

```

