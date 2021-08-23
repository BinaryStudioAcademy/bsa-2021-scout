using Application.Common.Files.Dtos;
using Domain.Interfaces.Read;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Applicants.Queries
{
    public class GetApplicantCvUrlQuery : IRequest<ApplicantCvDto>
    {
        public string ApplicantId { get; }

        public GetApplicantCvUrlQuery(string applicantID)
        {
            ApplicantId = applicantID;
        }
    }

    public class GetApplicantCvUrlQueryHandler : IRequestHandler<GetApplicantCvUrlQuery, ApplicantCvDto>
    {
        private readonly IApplicantCvFileReadRepository _applicantCvFileReadRepository;

        public GetApplicantCvUrlQueryHandler(IApplicantCvFileReadRepository applicantCvFileReadRepository)
        {
            _applicantCvFileReadRepository = applicantCvFileReadRepository;
        }

        public async Task<ApplicantCvDto> Handle(GetApplicantCvUrlQuery request, CancellationToken cancellationToken)
        {
            var applicantCvFileUrl = await _applicantCvFileReadRepository.GetSignedUrlAsync(request.ApplicantId);

            return new ApplicantCvDto(applicantCvFileUrl);
        }
    }
}
