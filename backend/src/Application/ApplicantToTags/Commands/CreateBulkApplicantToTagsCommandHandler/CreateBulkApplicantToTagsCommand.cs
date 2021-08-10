using Application.ApplicantToTags.Dtos;
using entities = Domain.Entities;
using Application.Common.Commands;
using Domain.Interfaces;
using AutoMapper;

namespace Application.ApplicantToTags.Commands.CreateBulkApplicantToTagsCommandHandler
{
    public class CreateBulkApplicantToTagsCommandHandler: CreateBulkElasticDocumentCommandHandler<entities::ApplicantToTags, CreateApplicantToTagsDto>
    {
         public CreateBulkApplicantToTagsCommandHandler(IElasticWriteRepository<entities::ApplicantToTags> repository, IMapper mapper ) : base(repository, mapper) { }
    }
}