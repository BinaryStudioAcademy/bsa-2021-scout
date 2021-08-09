using Domain.Interfaces.Read;
using Infrastructure.Files.Abstraction;
using Infrastructure.Files.Helpers;
using System.Threading.Tasks;
using FileInfo = Domain.Entities.FileInfo;

namespace Infrastructure.Files.Read
{
    public class ApplicantCvFileWriteRepository : IApplicantCvFileWriteRepository
    {
        private readonly IFileWriteRepository _fileWriteRepository;

        public ApplicantCvFileWriteRepository(IFileWriteRepository fileWriteRepository)
        {
            _fileWriteRepository = fileWriteRepository;
        }

        public Task<FileInfo> UploadAsync(string applicantId, byte[] cvFileContent)
        {
            return _fileWriteRepository.UploadPrivateFileAsync(
                ApplicantCvFileHelpers.GetFilePath(),
                ApplicantCvFileHelpers.GetFileName(applicantId),
                cvFileContent);
        }
    }
}
