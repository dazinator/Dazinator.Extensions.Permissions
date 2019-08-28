using System.Collections.Generic;

namespace Dazinator.Extensions.Permissions.Entities
{
    public interface IAppPermission<TAppPermission, TAppPermissionType>
        where TAppPermission : class, IAppPermission<TAppPermission, TAppPermissionType>, new()
        where TAppPermissionType : IAppPermissionType       
    {
        int Id { get; set; }
        int AppId { get; set; }
        //AppPermissionSubject<TAppPermission> AppPermissionSubject { get; set; }
        int AppPermissionSubjectId { get; set; }
        TAppPermissionType AppPermissionType { get; set; }
        int AppPermissionTypeId { get; set; }
        int? ParentId { get; set; }
        TAppPermission Parent { get; set; }
        ICollection<TAppPermission> Children { get; set; }
    }
}
