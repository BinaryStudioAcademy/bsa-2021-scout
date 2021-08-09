using Application.ApplicantToTags.Dtos;
using entities = Domain.Entities;
using Application.Common.Commands;
using Domain.Interfaces;
using AutoMapper;

namespace Application.ApplicantToTags.Commands.CreateApplicantToTagsCommandHandler
{
    public class CreateApplicantToTagsCommandHandler: CreateElasticDocumentCommandHandler<entities::ApplicantToTags, CreateApplicantToTagsDto>
    {
         public CreateApplicantToTagsCommandHandler(IElasticWriteRepository<entities::ApplicantToTags> repository, IMapper mapper) : base(repository, mapper) { }
    }
}