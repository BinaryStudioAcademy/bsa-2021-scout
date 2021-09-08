using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.AWS
{
    public interface ITextParser
    {
        Task<string> GetText(string jobId);
        Task<(string, string)> StartParsingAsync(byte[] fileContent);
        Task<(string, string)> StartParsingAsync(string fileContent);
        Task<(string, string)> StartParsingAsync(string fileContent, Encoding enconding);
    }
}
