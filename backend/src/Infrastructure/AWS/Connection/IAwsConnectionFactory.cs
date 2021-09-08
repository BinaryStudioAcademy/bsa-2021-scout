using Amazon.Runtime;

namespace Infrastructure.AWS.Connection
{
    public interface IAwsConnectionFactory
    {
        AWSCredentials GetCredentials();
        string GetRegion();
    }
}