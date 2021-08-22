using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Abstractions;

namespace Domain.Interfaces.Write
{
    public interface IVacancyCandidateWriteRepository
    {
        Task<IEnumerable<VacancyCandidate>> CreateRangeAsync(VacancyCandidate[] applicantIds);
    }
}
