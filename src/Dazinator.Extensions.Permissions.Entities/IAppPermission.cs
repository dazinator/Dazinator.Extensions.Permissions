namespace Dazinator.Extensions.Permissions.Entities
{
    public interface IAppPermission<TAppPermissionType> 
        where TAppPermissionType : IAppPermissionType       
    {
        int Id { get; set; }
        int AppId { get; set; }
        //AppPermissionSubject<TAppPermission> AppPermissionSubject { get; set; }
        int AppPermissionSubjectId { get; set; }
        TAppPermissionType AppPermissionType { get; set; }
        int AppPermissionTypeId { get; set; }
    }
}
