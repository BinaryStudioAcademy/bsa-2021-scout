using MediatR;
using System;
using System.Threading;
using Domain.Entities;
using System.Threading.Tasks;
using Domain.Interfaces;
using System.Collections.Generic;
using Application.Applicants.Dtos;
using Application.Common.Queries;
using Application.ApplicantToTags.Dtos;

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
                var applicantQueryTask = _mediator.Send(new GetEntityByIdQuery<ApplicantDto>(applicant.Id));
                var tagsQueryTask = _mediator.Send(new GetElasticDocumentByIdQuery<ApplicantToTagsDto>(applicant.Id));
                var vacancyInfoQueryTask = _mediator.Send(new GetVacancyInfoListQuery(applicant.Id));

                var resultApplicant = await applicantQueryTask;
                resultApplicant.Tags = await tagsQueryTask;
                resultApplicant.Vacancies = await vacancyInfoQueryTask;

                applicantResultList.Add(resultApplicant);
            }

            return applicantResultList;
        }
    }
}