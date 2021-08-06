using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Applicants.Dtos;
using Application.Common.Queries;
using Application.ApplicantToTags.Dtos;

namespace Application.Applicants.Queries
{
    public class GetComposedApplicantQuery : IRequest<ApplicantDto>
    {
        public string Id { get; private set; }

        public GetComposedApplicantQuery(string id)
        {
            Id = id;
        }
    }

    public class GetComposedApplicantQueryHandler : IRequestHandler<GetComposedApplicantQuery, ApplicantDto>
    {
        private readonly ISender _mediator;
        public GetComposedApplicantQueryHandler(ISender mediator)
        {
            _mediator = mediator;
        }
        public async Task<ApplicantDto> Handle(GetComposedApplicantQuery query, CancellationToken _)
        {
            var applicantQueryTask = _mediator.Send(new GetEntityByIdQuery<ApplicantDto>(query.Id));
            var tagsQueryTask = _mediator.Send(new GetElasticDocumentByIdQuery<ApplicantToTagsDto>(query.Id));
            var vacancyInfoQueryTask = _mediator.Send(new GetVacancyInfoListQuery(query.Id));

            var applicant = await applicantQueryTask;
            applicant.Tags = await tagsQueryTask;
            applicant.Vacancies = await vacancyInfoQueryTask;

            return applicant;
        }
    }
}