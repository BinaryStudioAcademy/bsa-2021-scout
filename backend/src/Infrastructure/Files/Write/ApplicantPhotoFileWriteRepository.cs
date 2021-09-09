using Application.Common.Exceptions.Applicants;
using Domain.Interfaces.Read;
using Domain.Interfaces.Write;
using Infrastructure.Files.Abstraction;
using System.IO;
using System.Threading.Tasks;
using FileInfo = Domain.Entities.FileInfo;

namespace Infrastructure.Files.Read
{
    public class ApplicantPhotoFileWriteRepository : IApplicantPhotoFileWriteRepository
    {
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IApplicantReadRepository _applicantReadRepository;

        public ApplicantPhotoFileWriteRepository(IFileWriteRepository fileWriteRepository, IApplicantReadRepository applicantReadRepository)
        {
            _fileWriteRepository = fileWriteRepository;
            _applicantReadRepository = applicantReadRepository;
        }

        public Task<FileInfo> UploadAsync(string applicantId, string extension, Stream photoFileContent)
        {
            return _fileWriteRepository.UploadPublicFileAsync(
                GetFilePath(),
                GetFileName(applicantId, extension),
                photoFileContent);
        }

        public async Task UpdateAsync(string applicantId, string extension, Stream photoFileContent)
        {
            var photoFileInfo = await _applicantReadRepository.GetPhotoFileInfoAsync(applicantId);
            await _fileWriteRepository.UpdateFileAsync(photoFileInfo, photoFileContent);
        }

        public async Task DeleteAsync(FileInfo photoFileInfo)
        {
            await _fileWriteRepository.DeleteFileAsync(photoFileInfo);
        }

        private static string GetFilePath()
        {
            return "applicant-photos";
        }

        private static string GetFileName(string applicantId, string extension)
        {
            return $"{applicantId}-photo{extension}";
        }
    }
}
