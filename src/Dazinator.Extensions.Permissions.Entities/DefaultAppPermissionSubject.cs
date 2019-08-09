using System.ComponentModel.DataAnnotations.Schema;

namespace Dazinator.Extensions.Permissions.Entities
{
    [Table("AppPermissionSubject")]
    public class DefaultAppPermissionSubject : AppPermissionSubject<DefaultAppPermission, DefaultAppPermissionType>
    {

    }  
}
