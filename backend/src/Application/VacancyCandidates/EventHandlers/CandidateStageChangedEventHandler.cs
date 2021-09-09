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
using Domain.Interfaces.Abstractions;
using Newtonsoft.Json;
using Application.MailTemplates.Queries;
using Application.Mail;
using Application.VacancyCandidates.Dtos;
using Application.Interviews.Commands.Create;
using Application.Interviews.Dtos;

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
        private readonly IReadRepository<Project> _projectReadRepository;
        protected readonly ICurrentUserContext _currentUserContext;
        private readonly ISender _mediator;
        private readonly IMapper _mapper;
        public CandidateStageChangedEventHandler(ILogger<CandidateStageChangedEventHandler> logger,
            IStageReadRepository stageReadRepository,
            IVacancyReadRepository vacancyReadRepository,
            IApplicantReadRepository applicantReadRepository,
            IVacancyCandidateReadRepository vacancyCandidateReadRepository,
            IReadRepository<Project> projectReadRepository,
        ICurrentUserContext currentUserContext,
            IMapper mapper,
        ISender mediator)
        {
            _logger = logger;
            _stageReadRepository = stageReadRepository;
            _vacancyReadRepository = vacancyReadRepository;
            _applicantReadRepository = applicantReadRepository;
            _vacancyCandidateReadRepository = vacancyCandidateReadRepository;
            _projectReadRepository = projectReadRepository;
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
            var project = await _projectReadRepository.GetAsync(vacancy.ProjectId);
            vacancy.Project = project;

            var user = _mapper.Map<User>(await _currentUserContext.GetCurrentUser());

            foreach (var action in stage.Actions)
            {
                if (action.StageChangeEventType == notification.Event.EventType)
                {
                    switch (action.ActionType)
                    {
                        case ActionType.AddTask:
                            await CreateTask(stage, vacancy, applicant);
                            break;
                        case ActionType.ScheduleInterviewAction:
                            await CreateInterviewTask(notification, stage, vacancy, applicant, user);
                            break;
                        case ActionType.SendMail:
                            await SendMailTask(notification, stage, vacancy, applicant);
                            break;
                    }
                }
            }
        }

        private async Task CreateTask(
            Stage stage, Vacancy vacancy, Applicant applicant)
        {
            var createTaskCommand = new CreateTaskCommand(
                new CreateTaskDto
                {
                    Name = $"Candidate {applicant.FirstName} {applicant.LastName} moved to stage {stage.Name} on vacancy {vacancy.Title}",
                    Note = "",
                    ApplicantId = applicant.Id,
                    DueDate = DateTime.Now,
                    UsersIds = new List<string>(),
                    IsReviewed = false
                });

            await _mediator.Send(createTaskCommand);
        }

        private async Task CreateInterviewTask(DomainEventNotification<CandidateStageChangedEvent> notification,
            Stage stage, Vacancy vacancy, Applicant applicant, User user)
        {
            var createInterviewTaskCommand = new CreateInterviewCommand(
            new CreateInterviewDto
            {
                Title = $"Interview with {applicant.FirstName} {applicant.LastName} on stage {stage.Name} on vacancy {vacancy.Title}",
                VacancyId = vacancy.Id,
                Scheduled = DateTime.Now,
                Duration = 30,
                InterviewType = Domain.Enums.InterviewType.Interview,
                CandidateId = applicant.Id,
                UserParticipants = new List<string> { user.Id },
                CreatedDate = DateTime.Now,
                IsReviewed = false
            });

            await _mediator.Send(createInterviewTaskCommand);
        }
        private async Task SendMailTask(DomainEventNotification<CandidateStageChangedEvent> notification,
            Stage stage, Vacancy vacancy, Applicant applicant)
        {
            var templatesIds = JsonConvert.DeserializeObject<TemplatesIdsDto>(stage.DataJson);
            var templateQuery = new GetMailTemplateWithReplacedPlaceholdersQuery(
                notification.Event.EventType == Domain.Enums.StageChangeEventType.Join ?
                templatesIds.JoinTemplateId : templatesIds.LeaveTemplateId,
                vacancy, applicant);
            var template = await _mediator.Send(templateQuery);

            var body = template.Html;
            var sendMailCommand = new SendMailCommand(applicant.Email, template.Subject, body, attachments: template.Attachments);
            await _mediator.Send(sendMailCommand);
        }
    }
}
