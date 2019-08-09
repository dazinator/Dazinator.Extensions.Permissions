using System.ComponentModel.DataAnnotations.Schema;

namespace Dazinator.Extensions.Permissions.Entities
{
    [Table("App")]
    public class DefaultApp : App<DefaultAppPermissionSubject, DefaultAppPermission, DefaultAppPermissionType>
    {

    }

    //[Table("AppPermission")]
    //public abstract class BasePermission<TAppPermission> : IAppPermission<TAppPermission>
    //    where TAppPermission : BasePermission<TAppPermission>
    //{
    //    public virtual int Id { get; set; }
    //    public AppPermissionSubject<TAppPermission> AppPermissionSubject { get; set; }
    //    public int AppPermissionSubjectId { get; set; }
    //    public AppPermissionType<TAppPermission> AppPermissionType { get; set; }
    //    public int AppPermissionTypeId { get; set; }
    //    public virtual int AppId { get; set; }
    //    public App<AppPermissionSubject<TAppPermission>, TAppPermission> App { get; set; }
    //}
}
