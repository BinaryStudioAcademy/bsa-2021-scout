using Application.ApplicantToTags.Dtos;
using entities = Domain.Entities;
using Application.Common.Commands;
using Domain.Interfaces;
using AutoMapper;

namespace Application.ApplicantToTags.Commands.UpdateApplicantToTagsCommandHandler
{
    public class UpdateApplicantToTagsCommandHandler: UpdateElasticDocumentCommandHandler<entities::ApplicantToTags, UpdateApplicantToTagsDto>
    {
         public UpdateApplicantToTagsCommandHandler(IElasticWriteRepository<entities::ApplicantToTags> repository, IMapper mapper) : base(repository, mapper) { }
    }
}