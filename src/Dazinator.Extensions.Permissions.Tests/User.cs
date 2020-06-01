using Dazinator.Extensions.Permissions.Entities;
using System.Collections.Generic;

namespace Dazinator.Extensions.Permissions.Tests
{
    public class User
    {
        public User()
        {
            Permissions = new HashSet<DefaultUserPermission>();
        }
        public int Id { get; set; }

        public ICollection<DefaultUserPermission> Permissions { get; set; }
    }

}
