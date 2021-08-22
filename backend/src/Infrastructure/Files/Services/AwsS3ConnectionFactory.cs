using System;
using Infrastructure.Files.Abstraction;
using Amazon.S3;

namespace Infrastructure.Mongo.Services
{
    public class AwsS3ConnectionFactory : IAwsS3ConnectionFactory
    {
        private readonly string _awsAccessKeyId;
        private readonly string _awsSecretAccessKey;
        private readonly string _awsRegion;
        private readonly string _bucketName;
        private IAmazonS3 _clientAmazonS3;

        public AwsS3ConnectionFactory()
        {
            _awsAccessKeyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            _awsSecretAccessKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            _awsRegion = Environment.GetEnvironmentVariable("AWS_REGION");
            _bucketName = Environment.GetEnvironmentVariable("AWS_S3_BUCKET_NAME");
        }

        public IAmazonS3 GetAwsS3()
        {
            if (_awsAccessKeyId != null && _awsSecretAccessKey != null && _awsRegion != null)
            {
                return _clientAmazonS3 ??= new AmazonS3Client(
                    awsAccessKeyId: _awsAccessKeyId,
                    awsSecretAccessKey: _awsSecretAccessKey,
                    region: Amazon.RegionEndpoint.GetBySystemName(_awsRegion));
            }

            return _clientAmazonS3 ??= new AmazonS3Client();
        }

        public string GetBucketName()
        {
            return _bucketName;
        }

        public string GetBucketRegion()
        {
            return _awsRegion;
        }
    }
}
