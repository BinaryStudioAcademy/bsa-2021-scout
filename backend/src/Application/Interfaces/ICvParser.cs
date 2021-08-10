using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICvParser
    {
        Task<Applicant> ParseAsync(string text, string lang = "en");
    }
}
