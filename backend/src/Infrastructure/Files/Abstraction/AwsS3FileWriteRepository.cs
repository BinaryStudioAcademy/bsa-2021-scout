using Amazon.S3;
using Amazon.S3.Transfer;
using Domain.Interfaces;
using Domain.Interfaces.Abstractions;
using Infrastructure.Files.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using FileInfo = Domain.Entities.FileInfo;

namespace Infrastructure.Files.Abstraction
{
    public class AwsS3FileWriteRepository : IFileWriteRepository
    {
        private readonly IAwsS3ConnectionFactory _awsS3Connection;
        private readonly IWriteRepository<FileInfo> _fileInfoRepository;
        private readonly IFileReadRepository _fileReadRepository;

        public AwsS3FileWriteRepository(IAwsS3ConnectionFactory awsS3Connection, IFileReadRepository fileReadRepository, IWriteRepository<FileInfo> fileInfoRepository)
        {
            _awsS3Connection = awsS3Connection;
            _fileInfoRepository = fileInfoRepository;
            _fileReadRepository = fileReadRepository;
        }

        public async Task<FileInfo> UploadPrivateFileAsync(string filePath, string fileName, Stream content)
        {
            await UploadAsync(filePath, fileName, content);

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
            await UploadAsync(filePath, fileName, content);

            var fileInfo = new FileInfo
            {
                Name = fileName,
                Path = filePath,
                PublicUrl = await _fileReadRepository.GetPublicUrlAsync(filePath, fileName)
            };

            await _fileInfoRepository.CreateAsync(fileInfo);

            return fileInfo;
        }

        private async Task UploadAsync(string filePath, string fileName, Stream content)
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = content,
                Key = AwsS3Helpers.GetFileKey(filePath, fileName),
                BucketName = _awsS3Connection.GetBucketName()
            };

            using var transferUtility = new TransferUtility(_awsS3Connection.GetAwsS3());

            await transferUtility.UploadAsync(uploadRequest);
        }
    }
}
