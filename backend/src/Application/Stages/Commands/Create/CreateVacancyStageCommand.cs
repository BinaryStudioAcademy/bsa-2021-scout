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
    public class CreateVacancyStageCommand : IRequest<StageDto>
    {
        public StageCreateDto StageCreate { get; set; }
        public string VacancyId { get; set; }

        public CreateVacancyStageCommand(StageCreateDto stCreate, string vacancyId)
        {
            StageCreate = stCreate;
            VacancyId = vacancyId;
        }
    }

    public class CreateVacancyStageCommandHandler : IRequestHandler<CreateVacancyStageCommand, StageDto>
    {
        private readonly IReadRepository<Vacancy> _readVacancyRepository;
        private readonly IWriteRepository<Stage> _writeRepository;
        private readonly IWriteRepository<ReviewToStage> _reviewToStageWriteRepository;
        private readonly IMapper _mapper;

        public CreateVacancyStageCommandHandler(
            IReadRepository<Vacancy> readVacancyRepository,
            IWriteRepository<Stage> writeRepository,
            IWriteRepository<ReviewToStage> reviewToStageWriteRepository,
            IMapper mapper
        )
        {
            _readVacancyRepository = readVacancyRepository;
            _writeRepository = writeRepository;
            _reviewToStageWriteRepository = reviewToStageWriteRepository;
            _mapper = mapper;
        }

        public async Task<StageDto> Handle(CreateVacancyStageCommand command, CancellationToken _)
        {
            if (await _readVacancyRepository.GetAsync(command.VacancyId) is null)
            {
                throw new Exception("This vacancy doesn't exist");
            }

            Stage stageToCreate = _mapper.Map<Stage>(command.StageCreate);
            Stage newStage = _mapper.Map<Stage>(command.StageCreate);

            stageToCreate.VacancyId = command.VacancyId;
            stageToCreate.ReviewToStages = null;
            await _writeRepository.CreateAsync(stageToCreate);

            foreach (ReviewToStage rts in newStage.ReviewToStages)
            {
                rts.StageId = stageToCreate.Id;
                rts.Review = null;
                await _reviewToStageWriteRepository.CreateAsync(rts);
            }

            StageDto createdStage = _mapper.Map<StageDto>(newStage);

            return createdStage;
        }
    }
}
