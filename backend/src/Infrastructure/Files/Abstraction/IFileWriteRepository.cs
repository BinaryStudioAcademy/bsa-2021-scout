using System.IO;
using System.Threading.Tasks;
using FileInfo = Domain.Entities.FileInfo;

namespace Infrastructure.Files.Abstraction
{
    public interface IFileWriteRepository
    {
        public Task<FileInfo> UploadPublicFileAsync(string filePath, string fileName, Stream content);
        public Task<FileInfo> UploadPrivateFileAsync(string filePath, string fileName, Stream content);
        public Task<FileInfo> UpdateFileAsync(FileInfo oldFileInfo, Stream content);
        public Task DeleteFileAsync(FileInfo fileInfo);
    }
}
