using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Tasks.Commands
{
    public class DeleteTaskCommand : IRequest
    {
        public string Id { get; }

        public DeleteTaskCommand(string id)
        {
            Id = id;
        }
    }

    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand>
    {
        protected readonly ISender _mediator;
        protected readonly IWriteRepository<ToDoTask> _TaskWriteRepository;
        protected readonly ISecurityService _securityService;
        protected readonly IMapper _mapper;

        public DeleteTaskCommandHandler(ISender mediator, IWriteRepository<ToDoTask> TaskWriteRepository, ISecurityService securityService, IMapper mapper)
        {
            _mediator = mediator;
            _TaskWriteRepository = TaskWriteRepository;            
            _securityService = securityService;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteTaskCommand command, CancellationToken _)
        {

            await _TaskWriteRepository.DeleteAsync(command.Id);

            return await Task.FromResult<Unit>(Unit.Value);

        }

        
    }
}
