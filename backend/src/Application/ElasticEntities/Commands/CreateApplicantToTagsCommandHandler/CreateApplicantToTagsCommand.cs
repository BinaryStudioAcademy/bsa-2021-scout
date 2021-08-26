using Application.ElasticEnities.Dtos;
using entities = Domain.Entities;
using Application.Common.Commands;
using Domain.Interfaces.Abstractions;
using AutoMapper;

namespace Application.ElasticEnities.Commands.CreateApplicantToTagsCommandHandler
{
    public class CreateApplicantToTagsCommandHandler : CreateElasticDocumentCommandHandler<entities::ElasticEntity, CreateElasticEntityDto>
    {
        public CreateApplicantToTagsCommandHandler(IElasticWriteRepository<entities::ElasticEntity> repository, IMapper mapper) : base(repository, mapper) { }
    }
}