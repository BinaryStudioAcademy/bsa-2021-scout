using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.Textract;
using Amazon.Textract.Model;
using Application.Interfaces.AWS;

namespace Infrastructure.AWS
{
    public class TextParser : ITextParser
    {
        private readonly IAmazonTextract _textract;

        public TextParser()
        {
            string keyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            string key = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            string region = Environment.GetEnvironmentVariable("AWS_REGION");

            _textract = new AmazonTextractClient(keyId, key, region);
        }

        public async Task<string> ParseAsync(byte[] fileContent)
        {
            MemoryStream stream = new MemoryStream(fileContent);
            DetectDocumentTextRequest request = new DetectDocumentTextRequest();
            request.Document.Bytes = stream;

            DetectDocumentTextResponse response = await _textract.DetectDocumentTextAsync(request);
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

        public async Task<string> ParseAsync(string fileContent)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(fileContent);
            return await ParseAsync(bytes);
        }

        public async Task<string> ParseAsync(string fileContent, Encoding enconding)
        {
            byte[] bytes = enconding.GetBytes(fileContent);
            return await ParseAsync(bytes);
        }
    }
}
