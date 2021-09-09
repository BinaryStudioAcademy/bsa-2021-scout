using Application.Common.Exceptions.Applicants;
using Domain.Interfaces.Read;
using Domain.Interfaces.Write;
using Infrastructure.Files.Abstraction;
using System.IO;
using System.Threading.Tasks;
using FileInfo = Domain.Entities.FileInfo;

namespace Infrastructure.Files.Read
{
    public class ApplicantCvFileWriteRepository : IApplicantCvFileWriteRepository
    {
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IApplicantReadRepository _applicantReadRepository;

        public ApplicantCvFileWriteRepository(IFileWriteRepository fileWriteRepository, IApplicantReadRepository applicantReadRepository)
        {
            _fileWriteRepository = fileWriteRepository;
            _applicantReadRepository = applicantReadRepository;
        }

        public Task<FileInfo> UploadAsync(string applicantId, Stream cvFileContent)
        {
            return _fileWriteRepository.UploadPublicFileAsync(
                GetFilePath(),
                GetFileName(applicantId),
                cvFileContent);
        }

        public async Task UpdateAsync(string applicantId, Stream cvFileContent)
        {
            var cvFileInfo = await _applicantReadRepository.GetCvFileInfoAsync(applicantId);
            await _fileWriteRepository.UpdateFileAsync(cvFileInfo, cvFileContent);
        }

        public async Task DeleteAsync(FileInfo cvFileInfo)
        {
            await _fileWriteRepository.DeleteFileAsync(cvFileInfo);
        }

        private static string GetFilePath()
        {
            return "applicants";
        }

        private static string GetFileName(string applicantId)
        {
            return $"{applicantId}-cv.pdf";
        }
    }
}
