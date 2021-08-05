using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Abstractions;

namespace Domain.Interfaces.Read
{
    public interface IApplicantCvReadRepository : IReadRepository<ApplicantCv>
    {
        Task<ApplicantCv> GetByApplicantAsync(string applicantId);
    }
}
