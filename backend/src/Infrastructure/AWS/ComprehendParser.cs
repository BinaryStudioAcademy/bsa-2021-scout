using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SharpCompress.Common;
using SharpCompress.Readers;
using Amazon;
using Amazon.Comprehend;
using Amazon.Comprehend.Model;
using common = Domain.Common;
using Application.Interfaces.AWS;
using Infrastructure.AWS.S3.Abstraction;

namespace Infrastructure.AWS
{
    public class ComprehendParser : IComprehendParser
    {
        private readonly IAmazonComprehend _comprehend;
        private readonly IAwsS3ConnectionFactory _awsS3ConnectionFactory;
        private readonly IS3Uploader _s3;
        private readonly string _skillsRecognizer;
        private readonly string _s3Role;

        public ComprehendParser(IS3Uploader s3, IAwsS3ConnectionFactory awsS3ConnectionFactory)
        {
            _s3 = s3;
            _awsS3ConnectionFactory = awsS3ConnectionFactory;

            _s3Role = Environment.GetEnvironmentVariable("AWS_COMPREHEND_S3_ROLE");
            _skillsRecognizer = Environment.GetEnvironmentVariable("AWS_COMPREHEND_SKILLS_RECOGNIZER");

            _comprehend = new AmazonComprehendClient();
        }

        public async Task<IEnumerable<common::TextEntity>> ParseEntitiesAsync(string text, string lang = "en")
        {
            DetectEntitiesRequest request = new DetectEntitiesRequest();
            request.Text = text;
            request.LanguageCode = lang;

            DetectEntitiesResponse response = await _comprehend.DetectEntitiesAsync(request);

            return ConvertEntities(response.Entities);

        }

        public async Task<(string, string)> StartParsingSkillsAsync(string text, string lang = "en")
        {
            string inputFileName = $"cv-text-{Guid.NewGuid().ToString()}.txt";
            string outputFolderName = $"cv-skills-result-{Guid.NewGuid().ToString()}";
            string inputFilePath = $"cv-texts/{inputFileName}";
            string outputFolderPath = $"cv-skills-results/{outputFolderName}";
            await _s3.UploadAsync(inputFilePath, Encoding.UTF8.GetBytes(text));

            StartEntitiesDetectionJobRequest request = new StartEntitiesDetectionJobRequest();
            request.DataAccessRoleArn = _s3Role;
            request.EntityRecognizerArn = _skillsRecognizer;
            request.LanguageCode = lang;
            request.InputDataConfig = new InputDataConfig();
            request.InputDataConfig.InputFormat = "ONE_DOC_PER_FILE";
            request.InputDataConfig.S3Uri = $"s3://{_awsS3ConnectionFactory.GetBucketName()}/{inputFilePath}";
            request.OutputDataConfig = new OutputDataConfig();
            request.OutputDataConfig.S3Uri = $"s3://{_awsS3ConnectionFactory.GetBucketName()}/{outputFolderPath}";

            await _comprehend.StartEntitiesDetectionJobAsync(request);

            return (inputFilePath, outputFolderPath);
        }

        public async Task<string> TarGZipOutputToString(byte[] tarGZipBytes)
        {
            MemoryStream gzipStream = new MemoryStream(tarGZipBytes);
            using GZipStream gzip = new GZipStream(gzipStream, CompressionMode.Decompress);

            StreamReader gzipReader = new StreamReader(gzip);
            string tar = await gzipReader.ReadToEndAsync();

            MemoryStream tarStream = new MemoryStream(Encoding.UTF8.GetBytes(tar));

            IReader tarReader = ReaderFactory.Open(tarStream);
            tarReader.MoveToNextEntry();

            EntryStream entryStream = tarReader.OpenEntryStream();
            StreamReader entryReader = new StreamReader(entryStream);

            return await entryReader.ReadToEndAsync();
        }

        public async Task<IEnumerable<common::TextEntity>> ParsePersonalDataAsync(string text, string lang = "en")
        {
            DetectPiiEntitiesRequest request = new DetectPiiEntitiesRequest();
            request.Text = text;
            request.LanguageCode = lang;

            DetectPiiEntitiesResponse response = await _comprehend.DetectPiiEntitiesAsync(request);

            return ConvertPiiEntities(text, response.Entities);
        }

        public IEnumerable<common::TextEntity> ConcatenateEntities(
            IEnumerable<common::TextEntity> ethalonEntities,
            params IEnumerable<common::TextEntity>[] otherEntities
        )
        {
            Regex newLineRegex = new Regex(@"\n|\r|\n\r|\r\n");

            List<common::TextEntity> resultEntities = ethalonEntities
                .SelectMany(e => newLineRegex
                    .Split(e.Text)
                    .Select(str => new common::TextEntity
                    {
                        Text = str.Trim(),
                        Type = e.Type,
                    }))
                .ToList();

            List<common::TextEntity> formattedOtherEntities = otherEntities
                .SelectMany(e => e)
                .SelectMany(e => newLineRegex
                    .Split(e.Text)
                    .Select(str => new common::TextEntity
                    {
                        Text = str.Trim(),
                        Type = e.Type,
                    }))
                .ToList();

            foreach (common::TextEntity other in formattedOtherEntities)
            {
                bool hasEntity = false;
                int index = 0;

                foreach (common::TextEntity entity in resultEntities)
                {
                    if (other.Text.ToLower().Trim() == entity.Text.ToLower().Trim())
                    {
                        hasEntity = true;
                        resultEntities[index].Type = other.Type;
                    }

                    index++;
                }

                if (!hasEntity)
                {
                    resultEntities.Add(other);
                }
            }

            return resultEntities;
        }

        private IEnumerable<common::TextEntity> ConvertEntities(IEnumerable<Entity> entities)
        {
            List<common::TextEntity> textEntities = new List<common::TextEntity>();

            foreach (Entity entity in entities)
            {
                textEntities.Add(new common::TextEntity
                {
                    Text = entity.Text,
                    Type = entity.Type,
                });
            }

            return textEntities;
        }

        private IEnumerable<common::TextEntity> ConvertPiiEntities(string text, IEnumerable<PiiEntity> entities)
        {
            List<common::TextEntity> textEntities = new List<common::TextEntity>();

            foreach (PiiEntity entity in entities)
            {
                textEntities.Add(new common::TextEntity
                {
                    Text = text.Substring(entity.BeginOffset, entity.EndOffset - entity.BeginOffset),
                    Type = entity.Type,
                });
            }

            return textEntities;
        }
    }
}
