using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Common;
using Application.Interfaces;
using Application.Interfaces.AWS;
using Application.Applicants.Dtos;

namespace Infrastructure.Services
{
    public class CvParser : ICvParser
    {
        private readonly IComprehendParser _comprehend;

        public CvParser(IComprehendParser comprehend)
        {
            _comprehend = comprehend;
        }

        public async Task<(string, string)> StartParsingSkillsAsync(string text)
        {
            return await _comprehend.StartParsingSkillsAsync(text);
        }

        public async Task<ApplicantCreationVariantsDto> FinishParsingAsync(string text, IEnumerable<TextEntity> skillEntities)
        {
            IEnumerable<TextEntity> generalEntities = await _comprehend.ParseEntitiesAsync(text);
            IEnumerable<TextEntity> personalDataEntities = await _comprehend.ParsePersonalDataAsync(text);

            IEnumerable<TextEntity> entities = _comprehend
                .ConcatenateEntities(generalEntities, skillEntities, personalDataEntities);

            IEnumerable<TextEntity> names = GetEntitiesByType("NAME", entities);
            IEnumerable<TextEntity> quantities = GetEntitiesByType("QUANTITY", entities);
            IEnumerable<TextEntity> phones = GetEntitiesByType("PHONE", entities);
            IEnumerable<TextEntity> emails = GetEntitiesByType("EMAIL", entities);
            IEnumerable<TextEntity> people = GetEntitiesByType("PERSON", entities);
            IEnumerable<TextEntity> skills = GetEntitiesByType("SKILL", entities);
            IEnumerable<TextEntity> organizations = GetEntitiesByType("ORGANIZATION", entities);
            IEnumerable<TextEntity> dates = GetEntitiesByType("DATE_TIME", entities);

            return new ApplicantCreationVariantsDto
            {
                FirstName = names
                    .Where(n => n.Text.Split(" ").Count() > 1)
                    .Select(n => n.Text.Split(" ")[0]),
                LastName = names
                    .Where(n => n.Text.Split(" ").Count() > 1)
                    .Select(n => n.Text.Split(" ")[1]),
                Experience = quantities
                    .Select(q => q.Text),
                Phone = phones
                    .Select(p => p.Text),
                Email = emails
                    .Select(e => e.Text),
                Skills = skills
                    .Select(s => s.Text),
                Company = organizations
                    .Select(o => o.Text),
                BirthDate = dates
                    .Select(d => d.Text),
            };
        }

        private IEnumerable<TextEntity> GetEntitiesByType(string type, IEnumerable<TextEntity> entities)
        {
            return entities.Where(e => e.Type == type);
        }
    }
}
