using System.Threading.Tasks;
using Domain.Interfaces.Abstractions;
using Domain.Entities;

namespace Domain.Interfaces.Read
{
    public interface IStageReadRepository : IReadRepository<Stage>
    {
        Task<Vacancy> GetByVacancyAsync(string vacancyId);
        Task<Stage> GetByVacancyIdWithFirstIndex(string vacancyId);
    }
}
