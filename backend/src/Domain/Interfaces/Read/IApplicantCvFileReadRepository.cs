using System.Threading.Tasks;

namespace Domain.Interfaces.Read
{
    public interface IApplicantCvFileReadRepository
    {
        Task<string> GetSignedUrlAsync(string applicantId);
    }
}