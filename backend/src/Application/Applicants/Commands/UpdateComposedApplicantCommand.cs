using MediatR;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Application.Applicants.Dtos;
using Application.Common.Commands;
using Application.ElasticEnities.Dtos;
using Application.Common.Queries;

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
        public UpdateComposedApplicantCommandHandler(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<ApplicantDto> Handle(UpdateComposedApplicantCommand command, CancellationToken _)
        {
            var updatableApplicant = _mapper.Map<ApplicantDto>(command.Entity);
            var query = new UpdateEntityCommand<ApplicantDto>(updatableApplicant);
            var updatedApplicant = await _mediator.Send(query);

            var elasticQuery = new GetElasticDocumentByIdQuery<ElasticEnitityDto>(updatedApplicant.Id);
            updatedApplicant.Tags = await _mediator.Send(elasticQuery);
            updatedApplicant.Vacancies = new List<ApplicantVacancyInfoDto>();

            return updatedApplicant;
        }
    }
}