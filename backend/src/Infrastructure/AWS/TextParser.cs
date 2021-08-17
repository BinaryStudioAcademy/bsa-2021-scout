using System;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.Textract;
using Amazon.Textract.Model;
using Application.Interfaces.AWS;

namespace Infrastructure.AWS
{
    public class TextParser : ITextParser
    {
        private readonly IAmazonTextract _textract;
        private readonly IS3Uploader _uploader;
        private readonly string _defaultBucket;
        private readonly string _role;
        private readonly string _topic;

        public TextParser(IS3Uploader uploader)
        {
            string keyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            string key = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            string region = Environment.GetEnvironmentVariable("AWS_REGION");

            _defaultBucket = Environment.GetEnvironmentVariable("AWS_DEFAULT_BUCKET");
            _role = Environment.GetEnvironmentVariable("AWS_TEXTRACT_SNS_ROLE");
            _topic = Environment.GetEnvironmentVariable("AWS_TEXTRACT_SNS_TOPIC");
            _textract = new AmazonTextractClient(keyId, key, RegionEndpoint.GetBySystemName(region));
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

        public async Task<string> StartParsingAsync(byte[] fileContent)
        {
            string fileName = $"cv-{Guid.NewGuid().ToString()}";
            string filePath = $"cvs/{fileName}.pdf";
            await _uploader.UploadAsync(filePath, fileContent);

            StartDocumentTextDetectionRequest request = new StartDocumentTextDetectionRequest();
            request.DocumentLocation = new DocumentLocation();
            request.DocumentLocation.S3Object = new S3Object();
            request.DocumentLocation.S3Object.Bucket = _defaultBucket;
            request.DocumentLocation.S3Object.Name = filePath;
            request.NotificationChannel = new NotificationChannel();
            request.NotificationChannel.RoleArn = _role;
            request.NotificationChannel.SNSTopicArn = _topic;
            Console.WriteLine(_role + " " + _topic + " " + _defaultBucket);

            StartDocumentTextDetectionResponse response = await _textract.StartDocumentTextDetectionAsync(request);
            return response.JobId;
        }

        public async Task<string> StartParsingAsync(string fileContent)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(fileContent);
            return await StartParsingAsync(bytes);
        }

        public async Task<string> StartParsingAsync(string fileContent, Encoding enconding)
        {
            byte[] bytes = enconding.GetBytes(fileContent);
            return await StartParsingAsync(bytes);
        }
    }
}
