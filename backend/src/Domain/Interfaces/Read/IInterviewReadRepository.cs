using Domain.Entities;
using System.Threading.Tasks;
using Domain.Interfaces.Abstractions;
using System.Collections.Generic;

namespace Domain.Interfaces.Read
{
    public interface IInterviewReadRepository : IReadRepository<Interview>
    {
        Task<IEnumerable<Interview>> GetInterviewsByCompanyIdAsync(string companyId);
        Task LoadInterviewersAsync(Interview interview);
    }
}
