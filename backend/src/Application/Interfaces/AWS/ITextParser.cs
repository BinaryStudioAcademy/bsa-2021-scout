using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.AWS
{
    public interface ITextParser
    {

        Task<string> StartParsingAsync(byte[] fileContent);
        Task<string> StartParsingAsync(string fileContent);
        Task<string> StartParsingAsync(string fileContent, Encoding enconding);
    }
}
