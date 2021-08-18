using System;
using MediatR;
using AutoMapper;
using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Application.Applicants.Dtos;
using Domain.Interfaces.Abstractions;
using System.Linq;
using Application.Common.Commands;
using Application.ElasticEnities.Dtos;

namespace Application.Applicants.Commands
{
    public class CreateComposedApplicantCommand : IRequest<ApplicantDto>
    {
        public CreateApplicantDto Entity { get; set; }
    
        public CreateComposedApplicantCommand(CreateApplicantDto entity)
        {
            Entity = entity;
        }
    }

    public class CreateComposedApplicantCommandHandler : IRequestHandler<CreateComposedApplicantCommand, ApplicantDto>
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;
        public CreateComposedApplicantCommandHandler(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<ApplicantDto> Handle(CreateComposedApplicantCommand command, CancellationToken _)
        {
            var newApplicant = _mapper.Map<ApplicantDto>(command.Entity);
            var query = new CreateEntityCommand<ApplicantDto>(newApplicant);
            var createdApplicant = await _mediator.Send(query);

            var elasticQuery = new CreateElasticDocumentCommand<CreateElasticEntityDto>(new CreateElasticEntityDto()
            {
                ElasticType = ElasticType.ApplicantTags,
                Id = createdApplicant.Id,
                TagsDtos = command.Entity.Tags.TagDtos.Select(t => new TagDto()
                {
                    Id = Guid.NewGuid().ToString(),
                    TagName = t.TagName
                })
            });
            createdApplicant.Tags = _mapper.Map<ElasticEnitityDto>(await _mediator.Send(elasticQuery));
            createdApplicant.Vacancies = new List<ApplicantVacancyInfoDto>();

            return createdApplicant;
        }
    }
}