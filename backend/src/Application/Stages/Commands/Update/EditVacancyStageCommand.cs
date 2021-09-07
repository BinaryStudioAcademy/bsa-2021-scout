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
using System.Collections.Generic;
using Action = Domain.Entities.Action;

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
        private readonly IWriteRepository<Action> _writeActionRepository;
        private readonly IMapper _mapper;

        public EditVacancyStageCommandHandler(
            IWriteRepository<Stage> writeStageRepository,
            IReadRepository<Vacancy> readVacancyRepository,
            IStageReadRepository readStageRepository,
            IWriteRepository<ReviewToStage> reviewToStageWriteRepository,
            IWriteRepository<Action> writeActionRepository,
            IMapper mapper
        )
        {
            _readVacancyRepository = readVacancyRepository;
            _writeStageRepository = writeStageRepository;
            _readStageRepository = readStageRepository;
            _reviewToStageWriteRepository = reviewToStageWriteRepository;
            _writeActionRepository = writeActionRepository;
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

            var existedStage = await _readStageRepository.GetWithReviews(command.StageId);
            var existedRts = existedStage.ReviewToStages;
            existedStage.Name = updateStage.Name;
            existedStage.Index = updateStage.Index;
            existedStage.Type = updateStage.Type;
            existedStage.IsReviewable = updateStage.IsReviewable;
            existedStage.Actions = updateStage.Actions;
            existedStage.DataJson = updateStage.DataJson;
            existedStage.ReviewToStages = null;

            existedStage.Actions = new List<Action>();
            foreach (var action in updateStage.Actions)
            {
                if(action.Id == null || action.Id == "")
                {
                    var addedAction = new Action()
                    {
                        ActionType = action.ActionType,
                        Name = action.Name,
                        StageId = command.StageId,
                        StageChangeEventType = action.StageChangeEventType
                    };
                    await _writeActionRepository.CreateAsync(addedAction);
                    existedStage.Actions.Add(addedAction);
                }
                else
                {
                    existedStage.Actions.Add(action);
                }
            }

            await _writeStageRepository.UpdateAsync(existedStage);
            existedStage.ReviewToStages = existedRts;

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
                var reviewsToStagesToDeleteIds = new List<string>();
                foreach (ReviewToStage rts in existedStage.ReviewToStages)
                {
                    if (updateStage.ReviewToStages.All(updateRts => updateRts.ReviewId != rts.ReviewId))
                    {
                        reviewsToStagesToDeleteIds.Add(rts.Id);
                    }
                }
                if (reviewsToStagesToDeleteIds.Count() != 0)
                {
                    foreach (var reviewToStagesToDeleteId in reviewsToStagesToDeleteIds)
                    {
                        if (existedStage.ReviewToStages.Count() != 0)
                        {
                            existedStage.ReviewToStages.Remove(existedStage.ReviewToStages.First(x => x.Id == reviewToStagesToDeleteId));
                        }
                        await _reviewToStageWriteRepository.DeleteAsync(reviewToStagesToDeleteId);
                    }
                }
            }

            var updatedVacancy = _mapper.Map<StageDto>(existedStage);

            return updatedVacancy;
        }
    }
}