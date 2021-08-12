using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Interfaces.Read
{
    public interface IApplicantReadRepository
    {
        Task<FileInfo> GetCvFileInfoAsync(string applicantId);
    }
}
