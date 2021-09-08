using System;
using Amazon;
using Amazon.S3;
using Infrastructure.AWS.Connection;
using Infrastructure.AWS.S3.Abstraction;

namespace Infrastructure.AWS.S3.Services
{
    public class AwsS3ConnectionFactory : IAwsS3ConnectionFactory
    {
        private string _bucketName;
        private IAmazonS3 _clientAmazonS3;
        private IAwsConnectionFactory _awsConnectionFactory;

        public AwsS3ConnectionFactory(IAwsConnectionFactory awsConnectionFactory)
        {
            _bucketName = Environment.GetEnvironmentVariable("AWS_S3_BUCKET_NAME");

            _awsConnectionFactory = awsConnectionFactory;
        }

        public IAmazonS3 GetAwsS3()
        {
            return _clientAmazonS3 ??= new AmazonS3Client();
        }

        public string GetBucketName()
        {
            return _bucketName;
        }

        public string GetBucketRegion()
        {
            return _awsConnectionFactory.GetRegion();
        }
    }
}
