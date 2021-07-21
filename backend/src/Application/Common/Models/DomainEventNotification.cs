using Domain.Common;
using MediatR;

namespace Application.Common.Models
{
    public class DomainEventNotification<TDomainEvent> : INotification where TDomainEvent : DomainEvent
    {
        public DomainEventNotification(TDomainEvent @event)
        {
            Event = @event;
        }

        public TDomainEvent Event { get; }
    }
}
