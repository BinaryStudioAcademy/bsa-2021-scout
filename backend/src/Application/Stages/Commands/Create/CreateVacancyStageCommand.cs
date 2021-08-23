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
        private readonly IMapper _mapper;

        public CreateVacancyStageCommandHandler(
             IReadRepository<Vacancy> readVacancyRepository,
            IWriteRepository<Stage> writeRepository,
            IMapper mapper
        )
        {
            _readVacancyRepository = readVacancyRepository;
            _writeRepository = writeRepository;
            _mapper = mapper;
        }

        public async Task<StageDto> Handle(CreateVacancyStageCommand command, CancellationToken _)
        {
            if (await _readVacancyRepository.GetAsync(command.VacancyId) is null)
            {
                throw new Exception("This vacancy doesn't exist");
            }

            var newStage = _mapper.Map<Stage>(command.StageCreate);

            newStage.VacancyId = command.VacancyId;
            
            await _writeRepository.CreateAsync(newStage);
            var registeredUser = _mapper.Map<StageDto>(newStage);

            return registeredUser;
        }
    }
}