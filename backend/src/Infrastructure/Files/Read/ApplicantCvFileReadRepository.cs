using Domain.Interfaces.Read;
using Infrastructure.Files.Abstraction;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Files.Read
{
    public class ApplicantCvFileReadRepository : IApplicantCvFileReadRepository
    {
        private readonly IFileReadRepository _fileReadRepository;
        private readonly IApplicantReadRepository _applicantReadRepository;

        public ApplicantCvFileReadRepository(IFileReadRepository fileReadRepository, IApplicantReadRepository applicantReadRepository)
        {
            _fileReadRepository = fileReadRepository;
            _applicantReadRepository = applicantReadRepository;
        }

        public async Task<string> GetSignedUrlAsync(string applicantId)
        {
            var applicantCvFileInfo = await _applicantReadRepository.GetCvFileInfoAsync(applicantId);

            return await _fileReadRepository.GetSignedUrlAsync(
                applicantCvFileInfo.Path,
                applicantCvFileInfo.Name,
                TimeSpan.FromMinutes(5));
        }
    }
}
