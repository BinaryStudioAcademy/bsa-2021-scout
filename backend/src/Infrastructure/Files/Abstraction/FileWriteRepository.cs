using Application.Interfaces.AWS;
using Domain.Interfaces.Abstractions;
using System.IO;
using System.Threading.Tasks;
using FileInfo = Domain.Entities.FileInfo;

namespace Infrastructure.Files.Abstraction
{
    public class FileWriteRepository : IFileWriteRepository
    {
        private readonly IAwsS3WriteRepository _awsS3WriteRepository;
        private readonly IAwsS3ReadRepository _awsS3ReadRepository;
        private readonly IWriteRepository<FileInfo> _fileInfoRepository;

        public FileWriteRepository(IAwsS3WriteRepository awsS3WriteRepository, IAwsS3ReadRepository awsS3ReadRepository, IWriteRepository<FileInfo> fileInfoRepository)
        {
            _awsS3WriteRepository = awsS3WriteRepository;
            _awsS3ReadRepository = awsS3ReadRepository;
            _fileInfoRepository = fileInfoRepository;
        }

        public async Task<FileInfo> UploadPrivateFileAsync(string filePath, string fileName, Stream content)
        {
            await _awsS3WriteRepository.UploadAsync(filePath, fileName, content);

            var fileInfo = new FileInfo
            {
                Name = fileName,
                Path = filePath,
            };

            await _fileInfoRepository.CreateAsync(fileInfo);

            return fileInfo;
        }

        public async Task<FileInfo> UploadPublicFileAsync(string filePath, string fileName, Stream content)
        {
            await _awsS3WriteRepository.UploadAsync(filePath, fileName, content);

            var fileInfo = new FileInfo
            {
                Name = fileName,
                Path = filePath,
                PublicUrl = await _awsS3ReadRepository.GetPublicUrlAsync(filePath, fileName)
            };

            await _fileInfoRepository.CreateAsync(fileInfo);

            return fileInfo;
        }

        public async Task<FileInfo> UpdateFileAsync(FileInfo oldFileInfo, Stream content)
        {
            await _awsS3WriteRepository.UploadAsync(oldFileInfo.Path, oldFileInfo.Name, content);

            return oldFileInfo;
        }

        public async Task DeleteFileAsync(FileInfo fileInfo)
        {
            await _awsS3WriteRepository.DeleteAsync(fileInfo.Path, fileInfo.Name);

            await _fileInfoRepository.DeleteAsync(fileInfo.Id);
        }
    }
}
