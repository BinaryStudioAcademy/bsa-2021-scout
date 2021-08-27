using System.IO;
using System.Threading.Tasks;
using FileInfo = Domain.Entities.FileInfo;

namespace Domain.Interfaces.Write
{
    public interface IMailAttachmentFileWriteRepository
    {
        Task UploadAsync(string key, Stream mailAttachmentFileContent);
        Task UpdateAsync(string applicantId, Stream cvFileContent);
        Task DeleteAsync(string filePath, string fileName);
    }
}