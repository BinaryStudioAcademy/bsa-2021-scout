using Domain.Entities.HomeData;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Read
{
    public interface IHomeDataReadRepository
    {
        public Task<WidgetsData> GetWidgetsDataAsync(string companyId);
        public Task<IEnumerable<HotVacancySummary>> GetHotVacancySummaryAsync(string companyId);
    }
}
