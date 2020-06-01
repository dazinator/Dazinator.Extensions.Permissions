using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dazinator.Extensions.Permissions.Entities
{
    [Table("AppPermission")]
    public class DefaultAppPermission : IAppPermission<DefaultAppPermission, DefaultAppPermissionType>
    {

        public DefaultAppPermission()
        {
            Children = new HashSet<DefaultAppPermission>();
        }

        //public virtual int Id { get; set; }
        //public int MinRequiredLicenceLevel { get; set; }
        public virtual int Id { get; set; }
        //public AppPermissionSubject<AppPermission> AppPermissionSubject { get; set; }
        //public int AppPermissionSubjectId { get; set; }
        public DefaultAppPermissionType AppPermissionType { get; set; }
        public int AppPermissionTypeId { get; set; }

        public int AppId { get; set; }
        public int AppPermissionSubjectId { get; set; }
        //public virtual int AppId { get; set; }
        //public App<AppPermissionSubject<AppPermission>, AppPermission> App { get; set; }

        public int? ParentId { get; set; }
        public virtual DefaultAppPermission Parent { get; set; }
        public virtual ICollection<DefaultAppPermission> Children { get; set; }

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
