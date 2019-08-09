using System;
using System.Collections.Generic;

namespace Dazinator.Extensions.Permissions
{
    public class AppPermissionsSeederOptions
    {
        public AppPermissionsSeederOptions()
        {
            AppPermissionTypes = new List<Type>();
        }

        public List<Type> AppPermissionTypes { get; set; }
    }


}
