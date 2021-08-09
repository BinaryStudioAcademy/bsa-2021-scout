using Domain.Interfaces.Read;
using Infrastructure.Files.Abstraction;
using Infrastructure.Files.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Files.Read
{
    public class ApplicantCvFileReadRepository : IApplicantCvFileReadRepository
    {
        private readonly IFileReadRepository _fileReadRepository;

        public ApplicantCvFileReadRepository(IFileReadRepository fileReadRepository)
        {
            _fileReadRepository = fileReadRepository;
        }

        public Task<string> GetSignedUrlAsync(string applicantId)
        {
            return _fileReadRepository.GetSignedUrlAsync(
                ApplicantCvFileHelpers.GetFilePath(),
                ApplicantCvFileHelpers.GetFileName(applicantId),
                TimeSpan.FromMinutes(5));
        }
    }
}
