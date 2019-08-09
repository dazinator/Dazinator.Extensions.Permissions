using System.Collections.Generic;

namespace Dazinator.Extensions.Permissions.Entities
{
    public interface IAppPermissionSubject<TAppPermission, TAppPermissionType>
        where TAppPermission : IAppPermission<TAppPermissionType>
        where TAppPermissionType : IAppPermissionType
    {
        int Id { get; set; }
        int AppId { get; set; }
        string Name { get; set; }
        HashSet<TAppPermission> Permissions { get; set; }
    }
}