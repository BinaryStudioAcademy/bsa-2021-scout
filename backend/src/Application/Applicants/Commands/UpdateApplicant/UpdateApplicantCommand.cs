using MediatR;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Application.Applicants.Dtos;
using Application.Common.Commands;
using Application.ElasticEnities.Dtos;
using Application.Common.Queries;
using Application.Common.Files.Dtos;

namespace Application.Applicants.Commands
{
    public class UpdateApplicantCommand : IRequest<ApplicantDto>
    {
        public UpdateApplicantDto Entity { get; set; }
        public FileDto CvFileDto { get; set; }

        public UpdateApplicantCommand(UpdateApplicantDto entity, FileDto cvFileDto)
        {
            Entity = entity;
            CvFileDto = cvFileDto;
        }
    }

    public class UpdateApplicantCommandHandler : IRequestHandler<UpdateApplicantCommand, ApplicantDto>
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;
        public UpdateApplicantCommandHandler(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<ApplicantDto> Handle(UpdateApplicantCommand command, CancellationToken _)
        {
            var updatableApplicant = _mapper.Map<ApplicantDto>(command.Entity);
            var query = new UpdateEntityCommand<ApplicantDto>(updatableApplicant);
            var updatedApplicant = await _mediator.Send(query);

            var elasticQuery = new GetElasticDocumentByIdQuery<ElasticEnitityDto>(updatedApplicant.Id);
            updatedApplicant.Tags = await _mediator.Send(elasticQuery);
            updatedApplicant.Vacancies = new List<ApplicantVacancyInfoDto>();

            await _mediator.Send(new UpdateApplicantCvCommand(updatableApplicant.Id, command.CvFileDto));

            return updatedApplicant;
        }
    }
}