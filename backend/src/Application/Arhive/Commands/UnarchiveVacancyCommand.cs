using System.Threading;
using System.Threading.Tasks;
using Application.Arhive.Dtos;
using Application.Common.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;

namespace Application.Archive.Commands
{
    public class UnarchiveVacancyCommand : IRequest<Unit>
    {
        public ArchivedVacancyDto ArchivedVacancy { get; set; }

        public UnarchiveVacancyCommand(ArchivedVacancyDto archivedVacancy)
        {
            ArchivedVacancy = archivedVacancy;
        }
    }
    public class UnarchiveVacancyCommandHandler : IRequestHandler<UnarchiveVacancyCommand, Unit>
    {
        private readonly IWriteRepository<ArchivedEntity> _archivedEntityWriteRepository;
        private readonly IWriteRepository<Vacancy> _vacancyWriteRepository;
        private readonly IReadRepository<Vacancy> _vacancyReadRepository;

        public UnarchiveVacancyCommandHandler(
            IWriteRepository<ArchivedEntity> archivedEntityWriteRepository,
            IWriteRepository<Vacancy> vacancyWriteRepository,
            IReadRepository<Vacancy> vacancyReadRepository)
        {
            _archivedEntityWriteRepository = archivedEntityWriteRepository;
            _vacancyWriteRepository = vacancyWriteRepository;
            _vacancyReadRepository = vacancyReadRepository;
        }
        public async Task<Unit> Handle(UnarchiveVacancyCommand command, CancellationToken cancellationToken)
        {
            await _archivedEntityWriteRepository.DeleteAsync(command.ArchivedVacancy.ArchivedVacancyData.Id);

            Vacancy vacancy = await _vacancyReadRepository.GetAsync(command.ArchivedVacancy.Id);
            if (vacancy is null)
            {
                throw new NotFoundException(typeof(Vacancy), command.ArchivedVacancy.Id);
            }
            vacancy.CompletionDate = null;
            await _vacancyWriteRepository.UpdateAsync(vacancy);

            return Unit.Value;
        }
    }
}