using Domain.Entities;
using System.Threading.Tasks;
using Domain.Interfaces.Abstractions;

namespace Domain.Interfaces.Read
{
    public interface IApplicantReadRepository : IReadRepository<Applicant>
    {
    }
}
