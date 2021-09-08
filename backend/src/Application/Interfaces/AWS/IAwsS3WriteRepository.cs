using System.IO;
using System.Threading.Tasks;

namespace Application.Interfaces.AWS
{
    public interface IAwsS3WriteRepository
    {
        Task UploadAsync(string filePath, byte[] fileContent);
        Task UploadAsync(string filePath, Stream fileContent);
        Task UploadAsync(string filePath, string fileName, byte[] fileContent);
        Task UploadAsync(string filePath, string fileName, Stream fileContent);
        Task DeleteAsync(string filePath, string fileName);
    }
}
