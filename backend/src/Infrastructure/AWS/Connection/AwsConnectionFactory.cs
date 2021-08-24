using Amazon;
using Amazon.Runtime;
using System;

namespace Infrastructure.AWS.Connection
{
    public class AwsConnectionFactory : IAwsConnectionFactory
    {
        private AWSCredentials _awsCredentials;
        private string _region;

        public AwsConnectionFactory()
        {
            InitializeCredentilas();
        }

        public AWSCredentials GetCredentials()
        {
            return _awsCredentials;
        }

        public string GetRegion()
        {
            return _region;
        }

        private void InitializeCredentilas()
        {
            SetCredentialsFromEnv();
        }

        private void SetCredentialsFromEnv()
        {
            if (_awsCredentials != null)
            {
                return;
            }

            string awsAccessKeyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            string awsSecretAccessKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            string awsRegion = Environment.GetEnvironmentVariable("AWS_REGION");

            if (awsAccessKeyId != null && awsSecretAccessKey != null && awsRegion != null)
            {
                AWSConfigs.AWSRegion = _region = awsRegion;
                _awsCredentials = new BasicAWSCredentials(awsAccessKeyId, awsSecretAccessKey);
            }
        }
    }
}
