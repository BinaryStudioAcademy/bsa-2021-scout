using Application.ApplicantToTags.Dtos;
using entities = Domain.Entities;
using Application.Common.Commands;
using Domain.Interfaces;
using AutoMapper;

namespace Application.ApplicantToTags.Commands.DeleteApplicantToTagsCommandHandler
{
    public class DeleteApplicantToTagsCommandHandler: DeleteElasticDocumentCommandHandler<entities::ApplicantToTags, ApplicantToTagsDto>
    {
         public DeleteApplicantToTagsCommandHandler(IElasticWriteRepository<entities::ApplicantToTags> repository) : base(repository) { }
    }
}