using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.AWS
{
    public interface ITextParser
    {
        Task<string> ParseAsync(byte[] fileContent);
        Task<string> ParseAsync(string fileContent);
        Task<string> ParseAsync(string fileContent, Encoding encoding);
    }
}
