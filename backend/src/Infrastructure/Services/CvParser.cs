using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Common;
using Domain.Entities;
using Application.Interfaces;
using Application.Interfaces.AWS;

namespace Infrastructure.Services
{
    public class CvParser : ICvParser
    {
        private readonly IComprehendParser _comprehend;

        public CvParser(IComprehendParser comprehend)
        {
            _comprehend = comprehend;
        }

        public async Task<Applicant> ParseAsync(string text, string lang = "en")
        {
            IEnumerable<TextEntity> generalEntities = await _comprehend.ParseEntitiesAsync(text, lang);
            IEnumerable<TextEntity> skillEntities = await _comprehend.ParseSkillsAsync(text, lang);
            IEnumerable<TextEntity> personalDataEntities = await _comprehend.ParsePersonalDataAsync(text, lang);

            IEnumerable<TextEntity> entities = _comprehend
                .ConcatenateEntities(generalEntities, skillEntities, personalDataEntities);

            return new Applicant();
        }
    }
}
