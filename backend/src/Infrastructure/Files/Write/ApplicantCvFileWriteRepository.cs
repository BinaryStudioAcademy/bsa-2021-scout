using Application.Common.Files.Dtos;
using Domain.Interfaces.Write;
using Infrastructure.Files.Abstraction;
using Infrastructure.Files.Helpers;
using System.IO;
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

        public Task<FileInfo> UploadAsync(string applicantId, Stream cvFileContent)
        {
            return _fileWriteRepository.UploadPrivateFileAsync(
                ApplicantCvFileHelpers.GetFilePath(),
                ApplicantCvFileHelpers.GetFileName(applicantId),
                cvFileContent);
        }
    }
}
