using AutoMapper;
using Domain.Entities;
using Application.Common.Queries;
using Application.Applicants.Dtos;
using Domain.Interfaces.Abstractions;

namespace Application.Applicants.Handlers
{
    public class GetApplicantByIdQueryHandler : GetEntitiesQueryHandler<Applicant, ApplicantDto>
    {
        public GetApplicantByIdQueryHandler(IReadRepository<Applicant> repository, IMapper mapper)
            : base(repository, mapper)
        { }
    }
}