using MediatR;
using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Application.Applicants.Dtos;
using Domain.Interfaces.Abstractions;

namespace Application.Applicants.Queries
{
    public class GetComposedApplicantListQuery : IRequest<IEnumerable<ApplicantDto>>
    { }

    public class GetComposedApplicantListQueryHandler : IRequestHandler<GetComposedApplicantListQuery, IEnumerable<ApplicantDto>>
    {
        private readonly IReadRepository<Applicant> _applicantRepository;
        private readonly ISender _mediator;
        public GetComposedApplicantListQueryHandler(IReadRepository<Applicant> applicantRepository, ISender mediator)
        {
            _mediator = mediator;
            _applicantRepository = applicantRepository;
        }
        public async Task<IEnumerable<ApplicantDto>> Handle(GetComposedApplicantListQuery query, CancellationToken _)
        {
            var applicantList = await _applicantRepository.GetEnumerableAsync();
            var applicantResultList = new List<ApplicantDto>();

            foreach (var applicant in applicantList)
            {
                applicantResultList.Add(await _mediator.Send(new GetComposedApplicantQuery(applicant.Id)));
            }

            return applicantResultList;
        }
    }
}