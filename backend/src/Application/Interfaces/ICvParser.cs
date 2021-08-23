using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Common;
using Application.Applicants.Dtos;

namespace Application.Interfaces
{
    public interface ICvParser
    {
        Task<(string, string)> StartParsingSkillsAsync(string text);
        Task<ApplicantCreationVariantsDto> FinishParsingAsync(string text, IEnumerable<TextEntity> skillEntities);
    }
}
