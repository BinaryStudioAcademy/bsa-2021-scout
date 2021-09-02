using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediatR;
using Domain.Events;
using Application.Common.Models;

namespace Application.VacancyCandidates.EventHandlers
{
    public class CandidateStageChangedEventHandler
        : INotificationHandler<DomainEventNotification<CandidateStageChangedEvent>>
    {
        private readonly ILogger<CandidateStageChangedEventHandler> _logger;

        public CandidateStageChangedEventHandler(ILogger<CandidateStageChangedEventHandler> logger)
        {
            _logger = logger;
        }

        public Task Handle(DomainEventNotification<CandidateStageChangedEvent> notification, CancellationToken _)
        {
            _logger.LogInformation(
                "Candidate with id {id} is now on stage with id {stageId} (vacancy with id {vacancyId})",
                notification.Event.Id,
                notification.Event.StageId,
                notification.Event.VacancyId
            );

            return Task.CompletedTask;
        }
    }
}
