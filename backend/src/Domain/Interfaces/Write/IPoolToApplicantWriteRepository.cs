using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Abstractions;

namespace Domain.Interfaces.Write
{
    public interface IPoolToApplicantWriteRepository : IWriteRepository<PoolToApplicant>
    {
        Task UpdatePoolApplicants(string [] newIds, string poolId, string name, string description);

        Task DeletePoolToApplicants(string id);
    }
}
