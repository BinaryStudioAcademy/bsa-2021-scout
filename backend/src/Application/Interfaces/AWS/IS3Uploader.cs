using System.IO;
using System.Threading.Tasks;

namespace Application.Interfaces.AWS
{
    public interface IS3Uploader
    {
        Task UploadAsync(string path, byte[] fileContent);
        Task UploadAsync(string path, Stream fileContent);
        Task UploadAsync(string bucket, string path, byte[] fileContent);
        Task UploadAsync(string bucket, string path, Stream fileContent);
        Task<byte[]> ReadAsync(string path);
        Task<byte[]> ReadAsync(string bucket, string path);
    }
}
