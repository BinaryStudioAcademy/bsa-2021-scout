using Application.ElasticEnities.Dtos;
using entities = Domain.Entities;
using Domain.Interfaces;
using AutoMapper;
using Application.Common.Queries;

namespace Application.ElasticEnities.Quaries.GetApplicantToTagsByIdQueryHandler
{
    public class GetApplicantToTagsByIdQueryHandler: GetElasticDocumentsListBySearchRequestQueryHandler<entities::ElasticEntity, ElasticEnitityDto>
    {
         public GetApplicantToTagsByIdQueryHandler(IElasticReadRepository<entities::ElasticEntity> repository, IMapper mapper) : base(repository, mapper) { }
    }
}