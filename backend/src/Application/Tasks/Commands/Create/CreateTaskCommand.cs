using Application.Interfaces;
using Application.Tasks.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands
{
    public class CreateTaskCommand : IRequest<TaskDto>
    {
        public CreateTaskDto NewTask { get; }

        public CreateTaskCommand(CreateTaskDto newTask)
        {
            NewTask = newTask;
        }
    }

    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
    {
        protected readonly ISender _mediator;
        protected readonly IWriteRepository<ToDoTask> _TaskWriteRepository;
        protected readonly IReadRepository<Applicant> _applicantReadRepository; 
        protected readonly ITaskReadRepository _TaskReadRepository;
        protected readonly ICurrentUserContext _userContext;
        protected readonly ISecurityService _securityService;
        protected readonly IMapper _mapper;

        public CreateTaskCommandHandler(ISender mediator, IWriteRepository<ToDoTask> TaskWriteRepository, ITaskReadRepository TaskReadRepository, ICurrentUserContext userContext, ISecurityService securityService, IMapper mapper)
        {
            _mediator = mediator;
            _TaskWriteRepository = TaskWriteRepository; 
            _TaskReadRepository = TaskReadRepository;
            _userContext = userContext;
            _securityService = securityService;
            _mapper = mapper;
        }

        public async Task<TaskDto> Handle(CreateTaskCommand command, CancellationToken _)
        {
            var newTask = _mapper.Map<ToDoTask>(command.NewTask);

            var currentUser = await _userContext.GetCurrentUser();

            if(currentUser!= null)
            {
                newTask.CompanyId = currentUser.CompanyId;
                newTask.CreatedById = currentUser.Id;
            }

            newTask.DateCreated = DateTime.Now;    
            
            var Task = await _TaskWriteRepository.CreateAsync(newTask);

            var createdTask = await _TaskReadRepository.GetTaskWithTeamMembersByIdAsync(Task.Id);

            return  _mapper.Map<TaskDto>(createdTask);

        }

        
    }
}
