using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces.Read
{
    public interface IApplicantCvFileWriteRepository
    {
        Task<FileInfo> UploadAsync(string applicantId, byte[] cvFileContent);
    }
}