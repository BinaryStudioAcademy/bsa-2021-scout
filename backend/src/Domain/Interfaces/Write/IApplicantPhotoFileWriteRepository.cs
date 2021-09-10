using System.IO;
using System.Threading.Tasks;
using FileInfo = Domain.Entities.FileInfo;

namespace Domain.Interfaces.Write
{
    public interface IApplicantPhotoFileWriteRepository
    {
        Task<FileInfo> UploadAsync(string applicantId, string exteinson, Stream cvFileContent);
        Task UpdateAsync(string applicantId, string extension, Stream cvFileContent);
        Task DeleteAsync(FileInfo fileInfo);
    }
}