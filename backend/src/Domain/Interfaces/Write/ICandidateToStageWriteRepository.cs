using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Abstractions;

namespace Domain.Interfaces.Write
{
    public interface ICandidateToStageWriteRepository : IWriteRepository<CandidateToStage>
    {
        Task ReplaceForCandidate(string candidateId, string newStageId);
    }
}
