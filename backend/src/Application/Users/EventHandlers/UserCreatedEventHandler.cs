using Application.Common.Models;
using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Users.EventHandlers
{
    public class UserCreatedEventHandler : INotificationHandler<DomainEventNotification<UserCreatedEvent>>
    {
        private readonly ILogger<UserCreatedEventHandler> _logger;

        public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<UserCreatedEvent> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("User with id {id} was created", notification.Event.User.Id);
            return Task.CompletedTask;
        }
    }
}
