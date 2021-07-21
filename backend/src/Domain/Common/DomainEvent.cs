using System;

namespace Domain.Common
{
    public abstract class DomainEvent
    {
        public DomainEvent()
        {
            DateOccurred = DateTime.UtcNow;
        }

        public bool IsPublished { get; set; }
        public DateTimeOffset DateOccurred { get; protected set; }
    }
}
