using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MediatR;
using Domain.Events;
using Application.Common.Models;
using Domain.Interfaces.Read;
using ActionType = Domain.Enums.ActionType;
using Application.Tasks.Commands;
using Application.Tasks.Dtos;
using Domain.Entities;
using System.Collections.Generic;
using Application.Interfaces;
using AutoMapper;
using System;

namespace Application.VacancyCandidates.EventHandlers
{
    public class CandidateStageChangedEventHandler
        : INotificationHandler<DomainEventNotification<CandidateStageChangedEvent>>
    {
        private readonly ILogger<CandidateStageChangedEventHandler> _logger;
        private readonly IStageReadRepository _stageReadRepository;
        private readonly IVacancyReadRepository _vacancyReadRepository;
        private readonly IApplicantReadRepository _applicantReadRepository;
        private readonly IVacancyCandidateReadRepository _vacancyCandidateReadRepository;
        protected readonly ICurrentUserContext _currentUserContext;
        private readonly ISender _mediator;
        private readonly IMapper _mapper;
        public CandidateStageChangedEventHandler(ILogger<CandidateStageChangedEventHandler> logger,
            IStageReadRepository stageReadRepository,
            IVacancyReadRepository vacancyReadRepository,
            IApplicantReadRepository applicantReadRepository,
            IVacancyCandidateReadRepository vacancyCandidateReadRepository,
            ICurrentUserContext currentUserContext,
            IMapper mapper,
        ISender mediator)
        {
            _logger = logger;
            _stageReadRepository = stageReadRepository;
            _vacancyReadRepository = vacancyReadRepository;
            _applicantReadRepository = applicantReadRepository;
            _vacancyCandidateReadRepository = vacancyCandidateReadRepository;
            _currentUserContext = currentUserContext;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task Handle(DomainEventNotification<CandidateStageChangedEvent> notification, CancellationToken _)
        {
            var stage = await _stageReadRepository.GetWithActions(notification.Event.StageId);
            var vacancy = await _vacancyReadRepository.GetAsync(notification.Event.VacancyId);
            var vacancyCandidate = await _vacancyCandidateReadRepository.GetAsync(notification.Event.Id);
            var applicant = await _applicantReadRepository.GetAsync(vacancyCandidate.ApplicantId);
            var user = _mapper.Map<User>(await _currentUserContext.GetCurrentUser());

            foreach (var action in stage.Actions)
            {
                if(action.StageChangeEventType == notification.Event.EventType)
                {
                    switch (action.ActionType)
                    {
                        case ActionType.AddTask:
                            await CreateTask(notification, stage, vacancy, applicant, user);
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


        private async Task CreateTask(DomainEventNotification<CandidateStageChangedEvent> notification, 
            Stage stage, Vacancy vacancy, Applicant applicant, User user)
        {
            var createTaskCommand = new CreateTaskCommand(
                new CreateTaskDto { 
                Name = $"Candidate {applicant.FirstName} {applicant.LastName} moved to stage {stage.Name} on vacancy {vacancy.Title}",
                Note = "",
                ApplicantId = applicant.Id,
                DueDate = DateTime.Now,
                UsersIds = new List<string>(),
                IsReviewed = false
              });

            await _mediator.Send(createTaskCommand);
        }
    }
}
