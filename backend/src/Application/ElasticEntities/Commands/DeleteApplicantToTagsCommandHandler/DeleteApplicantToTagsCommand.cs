using Application.ElasticEnities.Dtos;
using entities = Domain.Entities;
using Application.Common.Commands;
using Domain.Interfaces;
using AutoMapper;

namespace Application.ElasticEnities.Commands.DeleteApplicantToTagsCommandHandler
{
    public class DeleteApplicantToTagsCommandHandler: DeleteElasticDocumentCommandHandler<entities::ElasticEntity, ElasticEnitityDto>
    {
         public DeleteApplicantToTagsCommandHandler(IElasticWriteRepository<entities::ElasticEntity> repository) : base(repository) { }
    }
}