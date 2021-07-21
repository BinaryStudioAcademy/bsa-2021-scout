using System.Collections.Generic;

namespace Domain.Common.Interfaces
{
    public interface IHasDomainEvent
    {
        IList<DomainEvent> DomainEvents { get; set; }
    }
}
