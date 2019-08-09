using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dazinator.Extensions.Permissions.Entities
{
    [Table("AppPermissionType")]
    public class DefaultAppPermissionType : IAppPermissionType
    {

        public DefaultAppPermissionType()
        {
           // Permissions = new HashSet<TAppPermission>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual int Id { get; set; }

        [MaxLength(20)]
        [Required]
        public string Name { get; set; }

      //  public HashSet<TAppPermission> Permissions { get; set; }

    }
}
