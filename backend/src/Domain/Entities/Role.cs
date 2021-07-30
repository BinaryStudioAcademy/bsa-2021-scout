using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Role : Entity
    {
        public string Name { get; set; }
        public int Key { get; set; }

        public ICollection<UserToRole> RoleUsers { get; set; }
    }
}