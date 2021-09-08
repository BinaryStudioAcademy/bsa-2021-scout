using Application.ElasticEnities.Dtos;
using entities = Domain.Entities;
using Domain.Interfaces.Abstractions;
using AutoMapper;
using Application.Common.Queries;

namespace Application.ElasticEnities.Quaries.GetApplicantToTagsListBySearchRequestQueryHandler
{
    public class GetApplicantToTagsListBySearchRequestQueryHandler : GetElasticDocumentsListBySearchRequestQueryHandler<entities::ElasticEntity, ElasticEnitityDto>
    {
        public GetApplicantToTagsListBySearchRequestQueryHandler(IElasticReadRepository<entities::ElasticEntity> repository, IMapper mapper)
            : base(repository, mapper) { }
    }
}