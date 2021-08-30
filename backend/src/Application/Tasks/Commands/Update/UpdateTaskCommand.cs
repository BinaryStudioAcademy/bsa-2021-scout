using Application.Applicants.Dtos;
using Application.Common.Queries;
using Application.Interfaces;
using Application.Tasks.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using Domain.Interfaces.Write;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands
{
    public class UpdateTaskCommand : IRequest<TaskDto>
    {
        public UpdateTaskDto UpdateTask { get; }

        public UpdateTaskCommand(UpdateTaskDto updateTask)
        {
            UpdateTask = updateTask;
        }
    }

    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
    {
        protected readonly ISender _mediator;
        protected readonly IWriteRepository<ToDoTask> _TaskWriteRepository;
        protected readonly IUserToTaskWriteRepository _UserToTaskWriteRepository;
        protected readonly IReadRepository<Applicant> _applicantReadRepository;
        protected readonly ITaskReadRepository _TaskReadRepository;


        protected readonly ISecurityService _securityService;
        protected readonly IMapper _mapper;

        public UpdateTaskCommandHandler(ISender mediator, IWriteRepository<ToDoTask> TaskWriteRepository, IUserToTaskWriteRepository UserToTaskWriteRepository, ITaskReadRepository TaskReadRepository, ISecurityService securityService, IMapper mapper)
        {
            _mediator = mediator;
            _TaskWriteRepository = TaskWriteRepository;
            _UserToTaskWriteRepository = UserToTaskWriteRepository;
            _TaskReadRepository = TaskReadRepository;
            _securityService = securityService;
            _mapper = mapper;
        }

        public async Task<TaskDto> Handle(UpdateTaskCommand command, CancellationToken _)
        {

            var ids = command.UpdateTask.TeamMembersIds.Split(',', StringSplitOptions.RemoveEmptyEntries);
            
            await _UserToTaskWriteRepository.UpdateUsersToTask(ids, command.UpdateTask.Id, command.UpdateTask.Name, command.UpdateTask.Note);

            var resultTask = await _TaskReadRepository.GetTaskWithTeamMembersByIdAsync(command.UpdateTask.Id);

            return  _mapper.Map<TaskDto>(resultTask);

        }

        
    }
}
