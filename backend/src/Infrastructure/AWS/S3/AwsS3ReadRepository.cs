using Amazon.S3.Model;
using Application.Interfaces.AWS;
using Infrastructure.AWS.S3.Abstraction;
using Infrastructure.AWS.S3.Helpers;
using Infrastructure.Files.Abstraction;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.AWS.S3
{
    public class AwsS3ReadRepository : IAwsS3ReadRepository
    {
        private readonly IAwsS3ConnectionFactory _awsS3Connection;

        public AwsS3ReadRepository(IAwsS3ConnectionFactory awsS3Connection)
        {
            _awsS3Connection = awsS3Connection;
        }

        public Task<string> GetPublicUrlAsync(string filePath, string fileName)
        {
            var fileKey = AwsS3Helpers.GetFileKey(filePath, fileName);

            var publicUrl = $"http://{_awsS3Connection.GetBucketName()}.s3-{_awsS3Connection.GetBucketRegion()}.amazonaws.com/{fileKey}";

            return Task.FromResult(publicUrl);
        }

        public Task<string> GetSignedUrlAsync(string filePath, string fileName, TimeSpan timeSpan)
        {
            var preSignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = _awsS3Connection.GetBucketName(),
                Key = AwsS3Helpers.GetFileKey(filePath, fileName),
                Expires = DateTime.UtcNow.Add(timeSpan),
            };

            var preSignedUrl = _awsS3Connection.GetAwsS3().GetPreSignedURL(preSignedUrlRequest);
            return Task.FromResult(preSignedUrl);
        }

        public async Task<byte[]> ReadAsync(string filePath)
        {
            GetObjectResponse response = await _awsS3Connection.GetAwsS3().GetObjectAsync(_awsS3Connection.GetBucketName(), filePath);

            MemoryStream memory = new MemoryStream();
            response.ResponseStream.CopyTo(memory);

            return memory.ToArray();
        }

        public async Task<byte[]> ReadAsync(string filePath, string fileName)
        {
            var path = AwsS3Helpers.GetFileKey(filePath, fileName);

            return await ReadAsync(path);
        }
    }
}
