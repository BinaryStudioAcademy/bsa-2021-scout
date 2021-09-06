using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces.Abstractions;

namespace Domain.Interfaces.Read
{
    public interface ICandidateToStageReadRepository : IReadRepository<CandidateToStage>
    {
        Task<(IEnumerable<CandidateToStage>, bool)> GetRecentAsync(string userId, int page = 1);
        Task<IEnumerable<CandidateToStage>> GetRecentForApplicantAsync(string applicantId);
        Task<CandidateToStage> GetCurrentForCandidateByVacancyAsync(string candidateId, string vacancyId);
    }
}
