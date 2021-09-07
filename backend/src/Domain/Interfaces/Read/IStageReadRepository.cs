using System.Threading.Tasks;
using Domain.Interfaces.Abstractions;
using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Interfaces.Read
{
    public interface IStageReadRepository : IReadRepository<Stage>
    {
        Task<Vacancy> GetByVacancyAsync(string vacancyId);
        Task<Stage> GetByVacancyIdWithZeroIndex(string vacancyId);
        Task<Stage> GetByVacancyIdWithFirstIndex(string vacancyId);
        Task<IEnumerable<Stage>> GetByVacancyId(string vacancyId);
        Task<Stage> GetWithReviews(string id);
        Task<Stage> GetWithActions(string id);
    }
}
