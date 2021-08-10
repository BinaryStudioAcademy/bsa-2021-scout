using Application.ApplicantToTags.Dtos;
using entities = Domain.Entities;
using Domain.Interfaces;
using AutoMapper;
using Application.Common.Queries;

namespace Application.ApplicantToTags.Quaries.GetApplicantToTagsListBySearchRequestQueryHandler
{
    public class GetApplicantToTagsListBySearchRequestQueryHandler: GetElasticDocumentsListBySearchRequestQueryHandler<entities::ApplicantToTags, ApplicantToTagsDto>
    {
         public GetApplicantToTagsListBySearchRequestQueryHandler(IElasticReadRepository<entities::ApplicantToTags> repository, IMapper mapper) : base(repository, mapper) { }
    }
}