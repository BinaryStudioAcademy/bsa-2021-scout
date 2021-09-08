using Domain.Common;
using Domain.Enums;
using System;

namespace Domain.Entities
{
    public class ArchivedEntity: Entity
    {
        public string EntityId { get; set; }
        public EntityType EntityType { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
