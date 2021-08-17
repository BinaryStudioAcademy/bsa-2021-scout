using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Vacancies.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;

namespace Application.Vacancies.Commands.Edit
{
    public class EditVacancyCommand : IRequest<VacancyDto>
    {
        public VacancyUpdateDto VacancyUpdate { get; set; }
        public string Id { get; set; }

        public EditVacancyCommand(VacancyUpdateDto vacancyUpdate, string id)
        {
            VacancyUpdate = vacancyUpdate;
            Id = id;
        }
    }

    public class EditVacancyCommandHandler : IRequestHandler<EditVacancyCommand, VacancyDto>
    {
        private readonly IWriteRepository<Vacancy> _writeRepository;
        private readonly IReadRepository<Vacancy> _readRepository;
        private readonly IMapper _mapper;

        public EditVacancyCommandHandler(
            IWriteRepository<Vacancy> writeRepository,
            IReadRepository<Vacancy> readRepository,
            IMapper mapper
        )
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _mapper = mapper;
        }

        public async Task<VacancyDto> Handle(EditVacancyCommand command, CancellationToken _)
        {
            var updateVacancy = _mapper.Map<Vacancy>(command.VacancyUpdate);
            var existedVacancy = await _readRepository.GetAsync(command.Id);
            existedVacancy.Title = updateVacancy.Title;
            existedVacancy.Description = updateVacancy.Description;
            existedVacancy.Requirements = updateVacancy.Requirements;
            existedVacancy.ProjectId = updateVacancy.ProjectId;
            existedVacancy.SalaryFrom = updateVacancy.SalaryFrom;
            existedVacancy.SalaryTo = updateVacancy.SalaryTo;
            existedVacancy.TierFrom = updateVacancy.TierFrom;
            existedVacancy.TierTo = updateVacancy.TierTo;
            existedVacancy.Sources = updateVacancy.Sources;
            existedVacancy.IsHot = updateVacancy.IsHot;
            existedVacancy.IsRemote = updateVacancy.IsRemote;
            existedVacancy.Stages = updateVacancy.Stages;
            existedVacancy.ModificationDate = DateTime.Now;

            await _writeRepository.UpdateAsync(existedVacancy);
            var updatedVacancy = _mapper.Map<VacancyDto>(existedVacancy);

            return updatedVacancy;
        }
    }
}
