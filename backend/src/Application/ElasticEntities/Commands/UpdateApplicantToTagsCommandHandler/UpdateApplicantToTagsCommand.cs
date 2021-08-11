using Application.ElasticEnities.Dtos;
using entities = Domain.Entities;
using Application.Common.Commands;
using Domain.Interfaces.Abstractions;
using AutoMapper;

namespace Application.ElasticEnities.Commands.UpdateApplicantToTagsCommandHandler
{
    public class UpdateApplicantToTagsCommandHandler : UpdateElasticDocumentCommandHandler<entities::ElasticEntity, UpdateApplicantToTagsDto>
    {
        public UpdateApplicantToTagsCommandHandler(IElasticWriteRepository<entities::ElasticEntity> repository, IMapper mapper) : base(repository, mapper) { }
    }
}