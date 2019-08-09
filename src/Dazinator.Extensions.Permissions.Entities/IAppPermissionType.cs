using System.Collections.Generic;

namespace Dazinator.Extensions.Permissions.Entities
{
    public interface IAppPermissionType
    {
        int Id { get; set; }
        string Name { get; set; }
      //  HashSet<TAppPermission> Permissions { get; set; }
    }
}