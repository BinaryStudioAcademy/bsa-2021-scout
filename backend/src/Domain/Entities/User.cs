using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User: Human, IHasDomainEvent
    {
        public User()
        {
            DomainEvents = new List<DomainEvent>();
        }
        public UserStatus Status { get; set; }

        public IList<DomainEvent> DomainEvents { get; set; }
    }
}
