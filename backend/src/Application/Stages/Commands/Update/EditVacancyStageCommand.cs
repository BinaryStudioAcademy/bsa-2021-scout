using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Stages.Dtos;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Read;
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
        private readonly IStageReadRepository _readStageRepository;
        private readonly IReadRepository<Vacancy> _readVacancyRepository;
        private readonly IWriteRepository<ReviewToStage> _reviewToStageWriteRepository;
        private readonly IMapper _mapper;

        public EditVacancyStageCommandHandler(
            IWriteRepository<Stage> writeStageRepository,
            IReadRepository<Vacancy> readVacancyRepository,
            IStageReadRepository readStageRepository,
            IWriteRepository<ReviewToStage> reviewToStageWriteRepository,
            IMapper mapper
        )
        {
            _readVacancyRepository = readVacancyRepository;
            _writeStageRepository = writeStageRepository;
            _readStageRepository = readStageRepository;
            _reviewToStageWriteRepository = reviewToStageWriteRepository;
            _mapper = mapper;
        }

        public async Task<StageDto> Handle(EditVacancyStageCommand command, CancellationToken _)
        {
            var existedVacancy = await _readVacancyRepository.GetAsync(command.VacancyId);
            if (existedVacancy is null)
            {
                throw new Exception("The stage's vacancy doesn't exist");
            }

            var updateStage = _mapper.Map<Stage>(command.StageUpdate);

            var existedStage = await _readStageRepository.GetAsync(command.StageId);
            existedStage.Name = updateStage.Name;
            existedStage.Index = updateStage.Index;
            existedStage.Type = updateStage.Type;
            existedStage.IsReviewable = updateStage.IsReviewable;
            existedStage.Reviews = updateStage.Reviews;
            existedStage.Actions = updateStage.Actions;

            await _writeStageRepository.UpdateAsync(existedStage);

            foreach (ReviewToStage rts in updateStage.ReviewToStages)
            {
                if (existedStage.ReviewToStages.Any(existingRts => existingRts.ReviewId == rts.ReviewId))
                {
                    continue;
                }

                rts.StageId = updateStage.Id;
                rts.Review = null;
                await _reviewToStageWriteRepository.CreateAsync(rts);
            }

            if (updateStage.ReviewToStages.Count < existedStage.ReviewToStages.Count)
            {
                foreach (ReviewToStage rts in existedStage.ReviewToStages)
                {
                    if (updateStage.ReviewToStages.All(updateRts => updateRts.ReviewId != rts.ReviewId))
                    {
                        await _reviewToStageWriteRepository.DeleteAsync(rts.Id);
                    }
                }
            }

            var updatedVacancy = _mapper.Map<StageDto>(existedStage);

            return updatedVacancy;
        }
    }
}