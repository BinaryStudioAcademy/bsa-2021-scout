using Domain.Entities;
using System.Threading.Tasks;
using Domain.Interfaces.Abstractions;
using System.Collections.Generic;

namespace Domain.Interfaces.Read
{
    public interface IPoolReadRepository : IReadRepository<Pool>
    {
        Task<Pool> GetPoolWithApplicantsByIdAsync(string id);
        Task<List<Pool>> GetPoolsWithApplicantsAsync(string companyId);
        
    }
}
