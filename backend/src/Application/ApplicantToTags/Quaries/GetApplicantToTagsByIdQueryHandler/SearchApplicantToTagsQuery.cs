using Application.ApplicantToTags.Dtos;
using entities = Domain.Entities;
using Domain.Interfaces;
using AutoMapper;
using Application.Common.Queries;

namespace Application.ApplicantToTags.Quaries.GetApplicantToTagsByIdQueryHandler
{
    public class GetApplicantToTagsByIdQueryHandler: GetElasticDocumentsListBySearchRequestQueryHandler<entities::ApplicantToTags, ApplicantToTagsDto>
    {
         public GetApplicantToTagsByIdQueryHandler(IElasticReadRepository<entities::ApplicantToTags> repository, IMapper mapper) : base(repository, mapper) { }
    }
}