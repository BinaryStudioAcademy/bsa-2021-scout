using Domain.Common;

namespace Domain.Entities
{
    public class UserToRole : Entity
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public User User { get; private set; }
        public Role Role { get; private set; }
    }
}