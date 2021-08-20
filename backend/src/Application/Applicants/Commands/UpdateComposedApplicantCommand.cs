using MediatR;
using System;
using System.Linq;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Application.Applicants.Dtos;
using Application.Common.Commands;
using Application.ElasticEnities.Dtos;
using Application.Common.Queries;
using Domain.Entities;
using Domain.Interfaces.Read;

namespace Application.Applicants.Commands
{
    public class UpdateComposedApplicantCommand : IRequest<ApplicantDto>
    {
        public UpdateApplicantDto Entity { get; set; }
    
        public UpdateComposedApplicantCommand(UpdateApplicantDto entity)
        {
            Entity = entity;
        }
    }

    public class UpdateComposedApplicantCommandHandler : IRequestHandler<UpdateComposedApplicantCommand, ApplicantDto>
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicantsReadRepository _repository;
        public UpdateComposedApplicantCommandHandler(IApplicantsReadRepository repository, ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ApplicantDto> Handle(UpdateComposedApplicantCommand command, CancellationToken _)
        {
            var updatableApplicant = _mapper.Map<ApplicantDto>(command.Entity);
            var query = new UpdateEntityCommand<ApplicantDto>(updatableApplicant);
            var updatedApplicant = await _mediator.Send(query);

            var elasticQuery = new UpdateElasticDocumentCommand<UpdateApplicantToTagsDto>(
                _mapper.Map<UpdateApplicantToTagsDto>(command.Entity.Tags)
            );
            
            updatedApplicant.Tags = _mapper.Map<ElasticEnitityDto>(await _mediator.Send(elasticQuery));
            updatedApplicant.Vacancies = _mapper.Map<IEnumerable<ApplicantVacancyInfoDto>>
                (await _repository.GetApplicantVacancyInfoListAsync(updatedApplicant.Id));

            return updatedApplicant;
        }
    }
}