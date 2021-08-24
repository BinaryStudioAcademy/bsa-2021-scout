using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Application.Interfaces.AWS;
using Infrastructure.AWS.S3.Abstraction;
using Infrastructure.AWS.S3.Helpers;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Files.Abstraction
{
    public class AwsS3WriteRepository : IAwsS3WriteRepository
    {
        private readonly IAwsS3ConnectionFactory _awsS3Connection;

        public AwsS3WriteRepository(IAwsS3ConnectionFactory awsS3Connection)
        {
            _awsS3Connection = awsS3Connection;
        }

        public async Task UploadAsync(string filePath, byte[] fileContent)
        {
            Stream fileContentStream = new MemoryStream(fileContent);

            await UploadAsync(filePath, fileContentStream);
        }

        public async Task UploadAsync(string filePath, Stream fileContent)
        {
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = fileContent,
                Key = filePath,
                BucketName = _awsS3Connection.GetBucketName()
            };

            using var transferUtility = new TransferUtility(_awsS3Connection.GetAwsS3());

            await transferUtility.UploadAsync(uploadRequest);
        }

        public async Task UploadAsync(string filePath, string fileName, byte[] fileContent)
        {
            Stream fileContentStream = new MemoryStream(fileContent);

            await UploadAsync(AwsS3Helpers.GetFileKey(filePath, fileName), fileContentStream);
        }

        public async Task UploadAsync(string filePath, string fileName, Stream fileContent)
        {
            await UploadAsync(AwsS3Helpers.GetFileKey(filePath, fileName), fileContent);
        }

        public async Task DeleteAsync(string filePath, string fileName)
        {
            var deleteRequest = new DeleteObjectRequest
            {
                Key = AwsS3Helpers.GetFileKey(filePath, fileName),
                BucketName = _awsS3Connection.GetBucketName(),
            };

            await _awsS3Connection.GetAwsS3().DeleteObjectAsync(deleteRequest);
        }
    }
}
