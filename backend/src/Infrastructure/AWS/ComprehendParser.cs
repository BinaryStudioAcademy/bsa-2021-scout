using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Amazon;
using Amazon.Comprehend;
using Amazon.Comprehend.Model;
using common = Domain.Common;
using Application.Interfaces.AWS;

namespace Infrastructure.AWS
{
    public class ComprehendParser : IComprehendParser
    {
        private readonly IAmazonComprehend _comprehend;

        public ComprehendParser()
        {
            string keyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
            string key = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
            string region = Environment.GetEnvironmentVariable("AWS_REGION");

            _comprehend = new AmazonComprehendClient(keyId, key, RegionEndpoint.GetBySystemName(region));
        }

        public async Task<IEnumerable<common::TextEntity>> ParseEntitiesAsync(string text, string lang = "en")
        {
            DetectEntitiesRequest request = new DetectEntitiesRequest();
            request.Text = text;
            request.LanguageCode = lang;

            DetectEntitiesResponse response = await _comprehend.DetectEntitiesAsync(request);

            return ConvertEntities(response.Entities);

        }

        public async Task<IEnumerable<common::TextEntity>> ParseSkillsAsync(string text, string lang = "en")
        {
            string endpoint = Environment.GetEnvironmentVariable("AWS_COMPREHEND_SKILLS_ENDPOINT");

            DetectEntitiesRequest request = new DetectEntitiesRequest();
            request.Text = text;
            request.LanguageCode = lang;
            request.EndpointArn = endpoint;

            DetectEntitiesResponse response = await _comprehend.DetectEntitiesAsync(request);

            return ConvertEntities(response.Entities);

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
