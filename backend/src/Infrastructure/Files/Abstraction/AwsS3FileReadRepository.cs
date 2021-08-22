using Amazon.S3;
using Amazon.S3.Model;
using Domain.Entities;
using Infrastructure.Files.Helpers;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Files.Abstraction
{
    public class AwsS3FileReadRepository : IFileReadRepository
    {
        private readonly IAwsS3ConnectionFactory _awsS3Connection;

        public AwsS3FileReadRepository(IAwsS3ConnectionFactory awsS3Connection)
        {
            _awsS3Connection = awsS3Connection;
        }

        public Task<string> GetPublicUrlAsync(string filePath, string fileName)
        {
            var fileKey = AwsS3Helpers.GetFileKey(filePath, fileName);

            var publicUrl = $"https://{_awsS3Connection.GetBucketName()}.s3-{_awsS3Connection.GetBucketRegion()}.amazonaws.com/{fileKey}";

            return Task.FromResult(publicUrl);
        }

        public Task<string> GetSignedUrlAsync(string filePath, string fileName, TimeSpan timeSpan)
        {
            var preSignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = _awsS3Connection.GetBucketName(),
                Key = AwsS3Helpers.GetFileKey(filePath, fileName),
                Expires = DateTime.Now.Add(timeSpan),
            };

            var preSignedUrl = _awsS3Connection.GetAwsS3().GetPreSignedURL(preSignedUrlRequest);
            return Task.FromResult(preSignedUrl);
        }
    }
}
