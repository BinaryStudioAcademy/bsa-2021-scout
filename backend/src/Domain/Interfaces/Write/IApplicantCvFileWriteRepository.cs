using System.IO;
using System.Threading.Tasks;
using FileInfo = Domain.Entities.FileInfo;

namespace Domain.Interfaces.Write
{
    public interface IApplicantCvFileWriteRepository
    {
        Task<FileInfo> UploadAsync(string applicantId, Stream cvFileContent);
        Task UpdateAsync(string applicantId, Stream cvFileContent);
        Task DeleteAsync(FileInfo fileInfo);
    }
}