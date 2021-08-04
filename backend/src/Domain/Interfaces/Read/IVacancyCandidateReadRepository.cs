using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Abstractions;

namespace Domain.Interfaces.Read
{
    public interface IVacancyCandidateReadRepository : IReadRepository<VacancyCandidate>
    {
        Task<VacancyCandidate> GetFull(string id);
    }
}
