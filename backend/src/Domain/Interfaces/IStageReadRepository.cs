using System.Threading.Tasks;
using System.Collections.Generic;
using Domain.Interfaces.Abstractions;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IStageReadRepository : IReadRepository<Stage>
    {
        Task<IEnumerable<Stage>> GetByVacancy(string vacancyId);
    }
}
