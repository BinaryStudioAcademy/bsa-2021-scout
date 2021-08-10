using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Common;

namespace Application.Interfaces.AWS
{
    public interface IComprehendParser
    {
        Task<IEnumerable<TextEntity>> ParseEntitiesAsync(string text, string lang = "en");
        Task<IEnumerable<TextEntity>> ParseSkillsAsync(string text, string lang = "en");
        Task<IEnumerable<TextEntity>> ParsePersonalDataAsync(string text, string lang = "en");

        IEnumerable<TextEntity> ConcatenateEntities(
            IEnumerable<TextEntity> ethalonEntities,
            params IEnumerable<TextEntity>[] otherEntities
        );
    }
}
