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
    public class ArchiveVacancyCommand : IRequest<Unit>
    {
        public string VacancyId { get; set; }
        public bool IsClosed { get; set; }

        public ArchiveVacancyCommand(string vacancyId, bool isClosed = false)
        {
            VacancyId = vacancyId;
            IsClosed = isClosed;
        }
    }
    public class ArchiveVacancyCommandHandler : IRequestHandler<ArchiveVacancyCommand, Unit>
    {
        private readonly IWriteRepository<ArchivedEntity> _archivedEntityWriteRepository;
        private readonly IArchivedEntityReadRepository _archivedEntityReadRepository;
        private readonly IWriteRepository<Vacancy> _vacancyWriteRepository;
        private readonly IReadRepository<Vacancy> _vacancyReadRepository;
        private readonly ICurrentUserContext _currentUserContext;

        public ArchiveVacancyCommandHandler(
            IWriteRepository<ArchivedEntity> archivedEntityWriteRepository,
            IArchivedEntityReadRepository archivedEntityReadRepository,
            IWriteRepository<Vacancy> vacancyWriteRepository,
            IReadRepository<Vacancy> vacancyReadRepository,
            ICurrentUserContext currentUserContext)
        {
            _archivedEntityWriteRepository = archivedEntityWriteRepository;
            _archivedEntityReadRepository = archivedEntityReadRepository;
            _vacancyWriteRepository = vacancyWriteRepository;
            _vacancyReadRepository = vacancyReadRepository;
            _currentUserContext = currentUserContext;
        }
        public async Task<Unit> Handle(ArchiveVacancyCommand command, CancellationToken cancellationToken)
        {
            ArchivedEntity archivedVacancy = await _archivedEntityReadRepository.GetByEntityTypeAndIdAsync(EntityType.Vacancy, command.VacancyId);
            if (archivedVacancy is not null) 
            {
                throw new Exception();
            }

            var vacancy = await _vacancyReadRepository.GetAsync(command.VacancyId);
            if (vacancy is null)
            {
                throw new NotFoundException(typeof(Vacancy), command.VacancyId);
            }

            if (command.IsClosed)
            {
                vacancy.CompletionDate = DateTime.UtcNow;
                await _vacancyWriteRepository.UpdateAsync(vacancy);
            }

            archivedVacancy = new ArchivedEntity();
            archivedVacancy.UserId = (await _currentUserContext.GetCurrentUser()).Id;
            archivedVacancy.EntityType = EntityType.Vacancy;
            archivedVacancy.EntityId = command.VacancyId;
            await _archivedEntityWriteRepository.CreateAsync(archivedVacancy);
            return Unit.Value;
        }
    }
}