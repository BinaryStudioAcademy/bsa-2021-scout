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
        private readonly IAmazonS3 _clientAmazonS3;
        private readonly string _bucketName;

        public AwsS3FileReadRepository(IAmazonS3 clientAmazonS3)
        {
            _clientAmazonS3 = clientAmazonS3;
            _bucketName = Environment.GetEnvironmentVariable("AWS_S3_BUCKET_NAME");
        }

        public Task<string> GetSignedUrlAsync(string filePath, string fileName, TimeSpan timeSpan)
        {
            var preSignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = AwsS3Helpers.GetFileKey(filePath, fileName),
                Expires = DateTime.Now.Add(timeSpan),
            };

            var preSignedUrl = _clientAmazonS3.GetPreSignedURL(preSignedUrlRequest);
            return Task.FromResult(preSignedUrl);
        }
    }
}
