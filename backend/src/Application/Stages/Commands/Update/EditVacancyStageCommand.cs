using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Stages.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using MediatR;

namespace Application.Stages.Commands
{
    public class EditVacancyStageCommand : IRequest<StageDto>
    {
        public StageUpdateDto StageUpdate { get; set; }
        public string VacancyId { get; set; }
        public string StageId { get; set; }

        public EditVacancyStageCommand(StageUpdateDto stUpdate, string vacancyId, string stageId)
        {
            StageUpdate = stUpdate;
            VacancyId = vacancyId;
            StageId = stageId;
        }
    }

    public class EditVacancyStageCommandHandler : IRequestHandler<EditVacancyStageCommand, StageDto>
    {
        private readonly IWriteRepository<Stage> _writeStageRepository;
        private readonly IReadRepository<Stage> _readStageRepository;
        private readonly IReadRepository<Vacancy> _readVacancyRepository;
        private readonly IMapper _mapper;

        public EditVacancyStageCommandHandler(
            IWriteRepository<Stage> writeStageRepository,
            IReadRepository<Vacancy> readVacancyRepository,
            IReadRepository<Stage> readStageRepository,
            IMapper mapper
        )
        {
            _readVacancyRepository = readVacancyRepository;
            _writeStageRepository = writeStageRepository;
            _readStageRepository = readStageRepository;
            _mapper = mapper;
        }

        public async Task<StageDto> Handle(EditVacancyStageCommand command, CancellationToken _)
        {
            var existedVacancy = await _readVacancyRepository.GetAsync(command.VacancyId);
            if (existedVacancy is null)
            {
                throw new Exception("The stage's vacancy doesn't exist");
            }

            var updateVacancy = _mapper.Map<Stage>(command.StageUpdate);

            var existedStage = await _readStageRepository.GetAsync(command.StageId);
            existedStage.Name = updateVacancy.Name;
            existedStage.Index = updateVacancy.Index;
            existedStage.IsReviewable = updateVacancy.IsReviewable;
            existedStage.Reviews = updateVacancy.Reviews;
            existedStage.Actions = updateVacancy.Actions;

            await _writeStageRepository.UpdateAsync(existedStage);
            var updatedVacancy = _mapper.Map<StageDto>(existedStage);

            return updatedVacancy;
        }
    }
}