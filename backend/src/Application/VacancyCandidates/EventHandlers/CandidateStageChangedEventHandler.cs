using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediatR;
using Domain.Events;
using Application.Common.Models;
using Domain.Interfaces.Read;
using ActionType = Domain.Enums.ActionType;

namespace Application.VacancyCandidates.EventHandlers
{
    public class CandidateStageChangedEventHandler
        : INotificationHandler<DomainEventNotification<CandidateStageChangedEvent>>
    {
        private readonly ILogger<CandidateStageChangedEventHandler> _logger;
        private readonly IStageReadRepository _stageReadRepository;

        public CandidateStageChangedEventHandler(ILogger<CandidateStageChangedEventHandler> logger,
            IStageReadRepository stageReadRepository)
        {
            _logger = logger;
            _stageReadRepository = stageReadRepository;
        }

        public async Task Handle(DomainEventNotification<CandidateStageChangedEvent> notification, CancellationToken _)
        {
            var stage = await _stageReadRepository.GetWithActions(notification.Event.StageId);

            foreach(var action in stage.Actions)
            {
                if(action.StageChangeEventType == notification.Event.EventType)
                {
                    switch (action.ActionType)
                    {
                        case ActionType.AddTask:
                            _logger.LogInformation("Add task");
                            break;
                        case ActionType.ScheduleInterviewAction:
                            _logger.LogInformation("Schedule Interview Action");
                            break;
                        case ActionType.SendMail:
                            _logger.LogInformation("Send email");
                            break;
                    }
                }
            }
        }
    }
}
