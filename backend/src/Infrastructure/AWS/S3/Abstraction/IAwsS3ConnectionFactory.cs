using Amazon.S3;

namespace Infrastructure.AWS.S3.Abstraction
{
    public interface IAwsS3ConnectionFactory
    {
        IAmazonS3 GetAwsS3();
        string GetBucketName();
        string GetBucketRegion();
    }
}
