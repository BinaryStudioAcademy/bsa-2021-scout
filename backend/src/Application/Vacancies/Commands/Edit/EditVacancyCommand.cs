using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Commands;
using Application.ElasticEnities.Dtos;
using Application.Stages.Commands;
using Application.Stages.Dtos;
using Application.Stages.Queries;
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
        private readonly IReadRepository<Domain.Entities.Action> _readActionRepository;
        private readonly IWriteRepository<Domain.Entities.Action> _writeActionRepository;

        private readonly IMapper _mapper;
        private readonly ISender _mediator;

        public EditVacancyCommandHandler(
            IWriteRepository<Vacancy> writeRepository,
            IReadRepository<Vacancy> readRepository,
            IMapper mapper,
            ISender mediator,
            IReadRepository<Domain.Entities.Action> readActionRepository,
            IWriteRepository<Domain.Entities.Action> writeActionRepository
        )
        {
            _readRepository = readRepository;
            _writeRepository = writeRepository;
            _mapper = mapper;
            _mediator = mediator;
            _readActionRepository = readActionRepository;
            _writeActionRepository = writeActionRepository;
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
            existedVacancy.ModificationDate = DateTime.UtcNow;

            await _writeRepository.UpdateAsync(existedVacancy);

            var vacancyWithStages = await _mediator.Send(new GetStagesByVacancyQuery(command.Id));
            var stages = vacancyWithStages.Stages.ToList();
            var actions = await _readActionRepository.GetEnumerableAsync();
            if (updateVacancy.Stages.Count != 0)
            {
                foreach (var stage in updateVacancy.Stages)
                {
                    foreach(var action in stage.Actions)
                    {
                        action.StageId = stage.Id;
                    }
                    if (stage != null && (stage.Id == null || stage.Id == ""))
                    {
                        await _mediator.Send(new CreateVacancyStageCommand(_mapper.Map<StageCreateDto>(stage), command.Id));
                    }
                    if (stage != null && stages.Any(x => x.Id == stage.Id))
                    {
                        var thisStageActions = actions.Where(x => x.StageId == stage.Id).ToList();
                        foreach (var action in stage.Actions)
                        {
                            if (thisStageActions.Any(x => x.Id == action.Id))
                            {
                                thisStageActions.Remove(thisStageActions.First(x => x.Id == action.Id));
                            }
                            else
                            {
                                action.Id = null;
                            }
                        }
                        if (thisStageActions.Count() > 0)
                        {
                            foreach (var action in thisStageActions)
                            {
                                await _writeActionRepository.DeleteAsync(action.Id);
                            }
                        }
                        await _mediator.Send(new EditVacancyStageCommand(_mapper.Map<StageUpdateDto>(stage), command.Id, stage.Id));
                        stages.Remove(stages.FirstOrDefault(x => x.Id == stage.Id));
                    }
                }
            }
            if (stages.Count() != 0 || updateVacancy.Stages.Count == 0)
            {
                foreach (var stage in stages)
                {
                    await _mediator.Send(new DeleteVacancyStageCommand(stage.Id));
                }
            }

            command.VacancyUpdate.Tags.Id = existedVacancy.Id;

            var elasticQuery = new UpdateElasticDocumentCommand<UpdateApplicantToTagsDto>(
                _mapper.Map<UpdateApplicantToTagsDto>(command.VacancyUpdate.Tags)
            );
            var updatedVacancy = _mapper.Map<VacancyDto>(existedVacancy);

            updatedVacancy.Tags = _mapper.Map<ElasticEnitityDto>(await _mediator.Send(elasticQuery));

            return updatedVacancy;
        }
    }
}
