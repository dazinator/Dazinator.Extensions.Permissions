using System.ComponentModel.DataAnnotations.Schema;

namespace Dazinator.Extensions.Permissions.Entities
{
    [Table("RolePermission")]
    public class DefaultRolePermission : RolePermission<int, DefaultAppPermission, DefaultAppPermissionType>
    {

    }  
}
