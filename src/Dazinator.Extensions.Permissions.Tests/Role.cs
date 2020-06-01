using Dazinator.Extensions.Permissions.Entities;
using System.Collections.Generic;

namespace Dazinator.Extensions.Permissions.Tests
{
    public class Role
    {
        public Role()
        {
            Permissions = new HashSet<DefaultRolePermission>();
        }
        public int Id { get; set; }

        public ICollection<DefaultRolePermission> Permissions { get; set; }

    }

}
