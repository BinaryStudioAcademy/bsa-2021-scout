using Application.ElasticEnities.Dtos;
using entities = Domain.Entities;
using Application.Common.Commands;
using Domain.Interfaces.Abstractions;
using AutoMapper;

namespace Application.ElasticEnities.Commands.CreateBulkApplicantToTagsCommandHandler
{
    public class CreateBulkApplicantToTagsCommandHandler : CreateBulkElasticDocumentCommandHandler<entities::ElasticEntity, CreateElasticEntityDto>
    {
        public CreateBulkApplicantToTagsCommandHandler(IElasticWriteRepository<entities::ElasticEntity> repository, IMapper mapper) : base(repository, mapper) { }
    }
}