using System.Threading;
using System.Threading.Tasks;
using Application.Arhive.Dtos;
using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using MediatR;

namespace Application.Archive.Commands
{
    public class UnarchiveProjectCommand : IRequest<Unit>
    {
        public ArchivedProjectDto ArchivedProject { get; set; }

        public UnarchiveProjectCommand(ArchivedProjectDto archivedProject)
        {
            ArchivedProject = archivedProject;
        }
    }
    public class UnarchiveProjectCommandHandler : IRequestHandler<UnarchiveProjectCommand, Unit>
    {
        private readonly IWriteRepository<ArchivedEntity> _archivedEntityWriteRepository;
        private readonly IArchivedEntityReadRepository _archivedEntityReadRepository;
        private readonly IWriteRepository<Vacancy> _vacancyWriteRepository;
        private readonly IReadRepository<Vacancy> _vacancyReadRepository;

        public UnarchiveProjectCommandHandler(
            IWriteRepository<ArchivedEntity> archivedEntityWriteRepository,
            IArchivedEntityReadRepository archivedEntityReadRepository,
            IWriteRepository<Vacancy> vacancyWriteRepository,
            IReadRepository<Vacancy> vacancyReadRepository)
        {
            _archivedEntityWriteRepository = archivedEntityWriteRepository;
            _archivedEntityReadRepository = archivedEntityReadRepository;
            _vacancyWriteRepository = vacancyWriteRepository;
            _vacancyReadRepository = vacancyReadRepository;
        }
        public async Task<Unit> Handle(UnarchiveProjectCommand command, CancellationToken cancellationToken)
        {
            await _archivedEntityWriteRepository.DeleteAsync(command.ArchivedProject.ArchivedProjectData.Id);

            foreach (var vacancy in command.ArchivedProject.Vacancies)
            {
                var archivedVacancy = await _archivedEntityReadRepository.GetByEntityTypeAndIdAsync(EntityType.Vacancy, vacancy.Id);
                if (archivedVacancy is null)
                {
                    throw new NotFoundException(typeof(ArchivedEntity), vacancy.Id);
                }

                await _archivedEntityWriteRepository.DeleteAsync(archivedVacancy.Id);

                Vacancy unarchivedVacancy = await _vacancyReadRepository.GetAsync(vacancy.Id);
                if (vacancy is null)
                {
                    throw new NotFoundException(typeof(Vacancy), vacancy.Id);
                }
                unarchivedVacancy.CompletionDate = null;
                await _vacancyWriteRepository.UpdateAsync(unarchivedVacancy);
            }

            return Unit.Value;
        }
    }
}