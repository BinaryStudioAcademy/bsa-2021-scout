using System;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Textract;
using Amazon.Textract.Model;
using Application.Interfaces.AWS;
using Infrastructure.AWS.S3.Abstraction;

namespace Infrastructure.AWS
{
    public class TextParser : ITextParser
    {
        private readonly IAmazonTextract _textract;
        private readonly IS3Uploader _uploader;
        private readonly IAwsS3ConnectionFactory _awsS3ConnectionFactory;
        private readonly string _role;
        private readonly string _topic;

        public TextParser(IS3Uploader uploader, IAwsS3ConnectionFactory awsS3ConnectionFactory)
        {
            _role = Environment.GetEnvironmentVariable("AWS_TEXTRACT_SNS_ROLE");
            _topic = Environment.GetEnvironmentVariable("AWS_TEXTRACT_SNS_TOPIC");

            _awsS3ConnectionFactory = awsS3ConnectionFactory;
            _textract = new AmazonTextractClient();
            _uploader = uploader;
        }

        public async Task<string> GetText(string jobId)
        {
            GetDocumentTextDetectionRequest request = new GetDocumentTextDetectionRequest();
            request.JobId = jobId;

            GetDocumentTextDetectionResponse response = await _textract.GetDocumentTextDetectionAsync(request);
            string text = "";

            foreach (Block block in response.Blocks)
            {
                if (block.BlockType == "LINE")
                {
                    text += block.Text;
                    text += "\n";
                }
            }

            return text;
        }

        public async Task<(string, string)> StartParsingAsync(byte[] fileContent)
        {
            string fileName = $"cv-{Guid.NewGuid().ToString()}.pdf";
            string filePath = $"cvs/{fileName}";
            await _uploader.UploadAsync(filePath, fileContent);

            StartDocumentTextDetectionRequest request = new StartDocumentTextDetectionRequest();
            request.DocumentLocation = new DocumentLocation();
            request.DocumentLocation.S3Object = new S3Object();
            request.DocumentLocation.S3Object.Bucket = _awsS3ConnectionFactory.GetBucketName();
            request.DocumentLocation.S3Object.Name = filePath;
            request.NotificationChannel = new NotificationChannel();
            request.NotificationChannel.RoleArn = _role;
            request.NotificationChannel.SNSTopicArn = _topic;

            StartDocumentTextDetectionResponse response = await _textract.StartDocumentTextDetectionAsync(request);
            return (response.JobId, filePath);
        }

        public async Task<(string, string)> StartParsingAsync(string fileContent)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(fileContent);
            return await StartParsingAsync(bytes);
        }

        public async Task<(string, string)> StartParsingAsync(string fileContent, Encoding enconding)
        {
            byte[] bytes = enconding.GetBytes(fileContent);
            return await StartParsingAsync(bytes);
        }
    }
}
