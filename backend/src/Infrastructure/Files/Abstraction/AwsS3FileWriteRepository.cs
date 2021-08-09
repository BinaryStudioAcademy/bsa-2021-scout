using Amazon.S3;
using Amazon.S3.Transfer;
using Domain.Interfaces;
using Infrastructure.Files.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using FileInfo = Domain.Entities.FileInfo;

namespace Infrastructure.Files.Abstraction
{
    public class AwsS3FileWriteRepository : IFileWriteRepository
    {
        private readonly IAmazonS3 _clientAmazonS3;
        private readonly IWriteRepository<FileInfo> _fileInfoRepository;
        private readonly IFileReadRepository _fileReadRepository;
        private readonly string _bucketName;

        public AwsS3FileWriteRepository(IAmazonS3 clientAmazonS3, IFileReadRepository fileReadRepository, IWriteRepository<FileInfo> fileInfoRepository)
        {
            _clientAmazonS3 = clientAmazonS3;
            _fileInfoRepository = fileInfoRepository;
            _fileReadRepository = fileReadRepository;

            _bucketName = Environment.GetEnvironmentVariable("AWS_S3_BUCKET_NAME");
        }

        public async Task<FileInfo> UploadPrivateFileAsync(string filePath, string fileName, byte[] content)
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

        public async Task<FileInfo> UploadPublicFileAsync(string filePath, string fileName, byte[] content)
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

        private async Task UploadAsync(string filePath, string fileName, byte[] content)
        {
            var stream = new MemoryStream(content);

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = AwsS3Helpers.GetFileKey(filePath, fileName),
                BucketName = _bucketName
            };

            using var transferUtility = new TransferUtility(_clientAmazonS3);

            await transferUtility.UploadAsync(uploadRequest);
        }
    }
}
