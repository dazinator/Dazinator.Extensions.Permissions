using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dazinator.Extensions.Permissions.Entities
{
   // [Table("AppPermissionSubject")]
    public abstract class AppPermissionSubject<TAppPermission, TAppPermissionType> : IAppPermissionSubject<TAppPermission, TAppPermissionType> where TAppPermission : IAppPermission<TAppPermissionType>
        where TAppPermissionType : IAppPermissionType

    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int Id { get; set; }

        public AppPermissionSubject()
        {
            Permissions = new HashSet<TAppPermission>();
        }

        public int AppId { get; set; }
        //public App<AppPermissionSubject<TAppPermission>, TAppPermission> App { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public HashSet<TAppPermission> Permissions { get; set; }
    }


}
