using Application.Common.Models;
using Application.Interfaces;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DomainEventService : IDomainEventService
    {
        private readonly ILogger<DomainEventService> _logger;
        private readonly IPublisher _mediator;

        public DomainEventService(ILogger<DomainEventService> logger, IPublisher mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task Publish(DomainEvent @event)
        {
            _logger.LogInformation("Publishing domain event. Event - {event}", @event.GetType().Name);
            await _mediator.Publish(GetNotificationCorrespondingToDomainEvent(@event));
        }

        private INotification GetNotificationCorrespondingToDomainEvent(DomainEvent @event)
        {
            return (INotification)Activator.CreateInstance(
                typeof(DomainEventNotification<>).MakeGenericType(@event.GetType()), @event);
        }
    }
}
