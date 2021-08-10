using System.Threading.Tasks;
using Application.Applicants.Dtos;

namespace Application.Interfaces
{
    public interface ICvParser
    {
        Task<ApplicantCreationVariantsDto> ParseAsync(string text, string lang = "en");
    }
}
