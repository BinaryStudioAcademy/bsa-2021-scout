using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;

namespace Application.Archive.Commands
{
    public class ArchiveProjectCommand : IRequest<Unit>
    {
        public string ProjectId { get; set; }

        public ArchiveProjectCommand(string projectId)
        {
            ProjectId = projectId;
        }
    }
    public class ArchiveProjectCommandHandler : IRequestHandler<ArchiveProjectCommand, Unit>
    {
        private readonly IWriteRepository<ArchivedEntity> _archivedEntityWriteRepository;
        private readonly IArchivedEntityReadRepository _archivedEntityReadRepository;
        private readonly IReadRepository<Project> _projectReadRepository;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IMediator _mediator;

        public ArchiveProjectCommandHandler(
            IWriteRepository<ArchivedEntity> archivedEntityWriteRepository,
            IArchivedEntityReadRepository archivedEntityReadRepository,
            IReadRepository<Project> projectReadRepository,
            ICurrentUserContext currentUserContext,
            IMediator mediator)
        {
            _archivedEntityWriteRepository = archivedEntityWriteRepository;
            _archivedEntityReadRepository = archivedEntityReadRepository;
            _projectReadRepository = projectReadRepository;
            _currentUserContext = currentUserContext;
            _mediator = mediator;
        }
        public async Task<Unit> Handle(ArchiveProjectCommand command, CancellationToken cancellationToken)
        {
            ArchivedEntity archivedProject = await _archivedEntityReadRepository.GetByEntityTypeAndIdAsync(EntityType.Project, command.ProjectId);
            if (archivedProject is not null)
            {
                throw new Exception();
            }

            var project = await _projectReadRepository.GetAsync(command.ProjectId);
            if (project is null)
            {
                throw new NotFoundException(typeof(Project), command.ProjectId);
            }

            foreach (var vacancy in project.Vacancies)
            {
                await _mediator.Send(new ArchiveVacancyCommand(vacancy.Id));
            }

            archivedProject = new ArchivedEntity();
            archivedProject.UserId = (await _currentUserContext.GetCurrentUser()).Id;
            archivedProject.EntityType = EntityType.Project;
            archivedProject.EntityId = command.ProjectId;
            await _archivedEntityWriteRepository.CreateAsync(archivedProject);
            return Unit.Value;
        }
    }
}