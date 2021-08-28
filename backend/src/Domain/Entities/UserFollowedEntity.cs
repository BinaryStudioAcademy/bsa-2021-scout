using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class UserFollowedEntity: Entity
    {
        public string UserId { get; set; }
        public string EntityId { get; set; }
        public User User { get; set; }
        public EntityType EntityType { get; set; }
    }
}