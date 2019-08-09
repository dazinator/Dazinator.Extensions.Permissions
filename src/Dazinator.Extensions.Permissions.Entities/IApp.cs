using System.Collections.Generic;

namespace Dazinator.Extensions.Permissions.Entities
{
    public interface IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>
        where TAppPermissionSubject : IAppPermissionSubject<TAppPermission, TAppPermissionType>
        where TAppPermission : IAppPermission<TAppPermissionType>
        where TAppPermissionType : IAppPermissionType
    {
        string Code { get; set; }
        int Id { get; set; }
        HashSet<TAppPermission> Permissions { get; set; }
        HashSet<TAppPermissionSubject> Subjects { get; set; }
    }
}