using Domain.Entities;
using System.Threading.Tasks;
using Domain.Interfaces.Abstractions;
using System.Collections.Generic;

namespace Domain.Interfaces.Read
{
    public interface IVacancyTableReadRepository : IReadRepository<VacancyTable>
    {
        Task<IEnumerable<VacancyTable>> GetVacancyTablesByCompanyIdAsync(string companyId);
    }
}
