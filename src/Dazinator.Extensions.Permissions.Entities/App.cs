using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dazinator.Extensions.Permissions.Entities
{
    public abstract class App<TAppPermissionSubject, TAppPermission, TAppPermissionType> : IApp<TAppPermissionSubject, TAppPermission, TAppPermissionType> where TAppPermissionSubject : IAppPermissionSubject<TAppPermission, TAppPermissionType>
     where TAppPermissionType : IAppPermissionType
     where TAppPermission : IAppPermission<TAppPermissionType>
    {
        public virtual int Id { get; set; }
        public App()
        {
            Subjects = new HashSet<TAppPermissionSubject>();
            Permissions = new HashSet<TAppPermission>();
        }

        [Required]
        public string Code { get; set; }

        public HashSet<TAppPermissionSubject> Subjects { get; set; }

        public HashSet<TAppPermission> Permissions { get; set; }

    }
}
