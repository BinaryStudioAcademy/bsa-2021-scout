using System.IO;
using System.Threading.Tasks;

namespace Application.Interfaces.AWS
{
    public interface IS3Uploader
    {
        Task UploadAsync(string path, byte[] fileContent);
        Task UploadAsync(string path, Stream fileContent);
        Task<byte[]> ReadAsync(string path);
    }
}
