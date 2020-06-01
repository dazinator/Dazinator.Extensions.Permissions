using System.Collections.Generic;

namespace Dazinator.Extensions.Permissions.Entities
{
    public interface IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType>
        where TAppPermissionSubject : IAppPermissionSubject<TAppPermission, TAppPermissionType>
        where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
        where TAppPermissionType : IAppPermissionType
    {
        string Code { get; set; }
        int Id { get; set; }
        HashSet<TAppPermission> Permissions { get; set; }
        HashSet<TAppPermissionSubject> Subjects { get; set; }
    }

    public interface IUser<TAppPermissionSubject, TAppPermission, TAppPermissionType>
        where TAppPermissionSubject : IAppPermissionSubject<TAppPermission, TAppPermissionType>
        where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
        where TAppPermissionType : IAppPermissionType
    {      
        HashSet<TAppPermission> Permissions { get; set; }
        HashSet<TAppPermissionSubject> Subjects { get; set; }
    }
}