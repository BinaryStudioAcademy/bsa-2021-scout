using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;

namespace Application.Archive.Commands
{
    public class DeleteArhivedProjectCommand : IRequest<Unit>
    {
        public string ProjectId { get; set; }

        public DeleteArhivedProjectCommand(string projectId)
        {
            ProjectId = projectId;
        }
    }
    public class DeleteArhivedProjectCommandHandler : IRequestHandler<DeleteArhivedProjectCommand, Unit>
    {
        private readonly IWriteRepository<ArchivedEntity> _archivedEntityWriteRepository;
        private readonly IArchivedEntityReadRepository _archivedEntityReadRepository;
        private readonly IWriteRepository<UserFollowedEntity> _userFollowedEntityWriteRepository;
        private readonly IUserFollowedReadRepository _userFollowedEntityReadRepository;
        private readonly IWriteRepository<Project> _projectWriteRepository;
        private readonly IReadRepository<Vacancy> _vacancyReadRepository;
        private readonly IElasticWriteRepository<ElasticEntity> _projectElasticWriteRepository;
        private readonly ISender _mediator;

        public DeleteArhivedProjectCommandHandler(
            IWriteRepository<ArchivedEntity> archivedEntityWriteRepository,
            IArchivedEntityReadRepository archivedEntityReadRepository,
            IWriteRepository<UserFollowedEntity> userFollowedEntityWriteRepository,
            IUserFollowedReadRepository userFollowedEntityReadRepository,
            IWriteRepository<Project> projectWriteRepository,
            IReadRepository<Vacancy> vacancyReadRepository,
            IElasticWriteRepository<ElasticEntity> projectElasticWriteRepository,
            ISender mediator)
        {
            _archivedEntityWriteRepository = archivedEntityWriteRepository;
            _archivedEntityReadRepository = archivedEntityReadRepository;
            _userFollowedEntityWriteRepository = userFollowedEntityWriteRepository;
            _userFollowedEntityReadRepository = userFollowedEntityReadRepository;
            _projectWriteRepository = projectWriteRepository;
            _vacancyReadRepository = vacancyReadRepository;
            _projectElasticWriteRepository = projectElasticWriteRepository;
            _mediator = mediator;
        }
        public async Task<Unit> Handle(DeleteArhivedProjectCommand command, CancellationToken cancellationToken)
        {
            var archivedProject = await _archivedEntityReadRepository.GetByEntityTypeAndIdAsync(EntityType.Project, command.ProjectId);
            if (archivedProject is not null)
            {
                await _archivedEntityWriteRepository.DeleteAsync(archivedProject.Id);
            }

            var followedProject = await _userFollowedEntityReadRepository.GetForCurrentUserByTypeAndEntityId(command.ProjectId, EntityType.Project);
            if (followedProject is not null)
            {
                await _userFollowedEntityWriteRepository.DeleteAsync(followedProject.Id);
            }
           
            IEnumerable<string> vacanciesIds = (await _vacancyReadRepository.GetEnumerableByPropertyAsync(nameof(Vacancy.ProjectId), command.ProjectId))
                                                .Select(vacancy => vacancy.Id);

            foreach (var vacancyId in vacanciesIds)
            {
                await _mediator.Send(new DeleteArhivedVacancyCommand(vacancyId));
            }

            await _projectWriteRepository.DeleteAsync(command.ProjectId);
            await _projectElasticWriteRepository.DeleteAsync(command.ProjectId);

            return Unit.Value;
        }
    }
}