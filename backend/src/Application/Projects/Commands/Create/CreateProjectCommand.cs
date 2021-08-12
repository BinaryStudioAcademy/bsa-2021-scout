using Application.Interfaces;
using Application.Projects.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Projects.Commands.Create
{
    public class CreateProjectCommand : IRequest<ProjectDto>
    {
        public ProjectDto Project { get; }

        public CreateProjectCommand(ProjectDto project)
        {
            Project = project;
        }
    }

    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
    {
        protected readonly IWriteRepository<Project> _repository;
        protected readonly ICurrentUserContext _currentUserContext;
        protected readonly IMapper _mapper;

        public CreateProjectCommandHandler(IWriteRepository<Project> repository, ICurrentUserContext currentUserContext, IMapper mapper)
        {
            _repository = repository;
            _currentUserContext = currentUserContext;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Handle(CreateProjectCommand command, CancellationToken _)
        {
            Project entity = _mapper.Map<Project>(command.Project);

            entity.Id = Guid.NewGuid().ToString();
            entity.CreationDate = DateTime.Now;
            entity.CompanyId = (await _currentUserContext.GetCurrentUser()).CompanyId;

            var created = await _repository.CreateAsync(entity);

            return _mapper.Map<ProjectDto>(created);
        }
    }
}
