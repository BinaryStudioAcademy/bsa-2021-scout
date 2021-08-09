using Domain.Entities;
using System.Threading.Tasks;

namespace Infrastructure.Files.Abstraction
{
    public interface IFileWriteRepository
    {
        public Task<FileInfo> UploadPublicFileAsync(string filePath, string fileName, byte[] content);
        public Task<FileInfo> UploadPrivateFileAsync(string filePath, string fileName, byte[] content);
    }
}
