using System.ComponentModel.DataAnnotations.Schema;

namespace Dazinator.Extensions.Permissions.Entities
{
    [Table("UserPermission")]
    public class DefaultUserPermission : UserPermission<int, DefaultAppPermission, DefaultAppPermissionType>
    { 
    
    }
}
