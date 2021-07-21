using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User: Entity, IHasDomainEvent
    {
        public User()
        {
            DomainEvents = new List<DomainEvent>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birth { get; set; }
        public UserStatus Status { get; set; }

        public IList<DomainEvent> DomainEvents { get; set; }
    }
}
