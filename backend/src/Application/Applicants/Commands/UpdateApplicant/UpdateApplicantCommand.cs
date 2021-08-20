using MediatR;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Application.Applicants.Dtos;
using Application.Common.Commands;
using Application.ElasticEnities.Dtos;
using Application.Common.Files.Dtos;
using Domain.Interfaces.Read;

namespace Application.Applicants.Commands
{
    public class UpdateApplicantCommand : IRequest<ApplicantDto>
    {
        public UpdateApplicantDto Entity { get; set; }
        public FileDto? CvFileDto { get; set; }

        public UpdateApplicantCommand(UpdateApplicantDto entity, FileDto? cvFileDto)
        {
            Entity = entity;
            CvFileDto = cvFileDto;
        }
    }

    public class UpdateApplicantCommandHandler : IRequestHandler<UpdateApplicantCommand, ApplicantDto>
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;
        private readonly IApplicantReadRepository _repository;

        public UpdateApplicantCommandHandler(IApplicantReadRepository repository, ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ApplicantDto> Handle(UpdateApplicantCommand command, CancellationToken _)
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

            await UploadCvFileIfExists(updatableApplicant, command);

            return updatedApplicant;
        }

        private async Task UploadCvFileIfExists(ApplicantDto updatableApplicant, UpdateApplicantCommand command)
        {
            if (command.CvFileDto != null)
            {
                return;
            }

            await _mediator.Send(new UpdateApplicantCvCommand(updatableApplicant.Id, command.CvFileDto!));
        }
    }
}