using Domain.Entities;
using System.Threading.Tasks;
using Domain.Interfaces.Abstractions;
using System.Collections.Generic;
using Domain.Enums;
using System;

namespace Domain.Interfaces.Read
{
    public interface IArchivedEntityReadRepository : IReadRepository<ArchivedEntity>
    {
        Task<ArchivedEntity> GetByEntityTypeAndIdAsync(EntityType entityType, string entityId);
        Task<IEnumerable<Tuple<Project, ArchivedEntity>>> GetArchivedProjectsAsync(string companyId);
        Task<IEnumerable<Tuple<Vacancy, ArchivedEntity, ArchivedEntity>>> GetArchivedVacanciesAsync(string companyId);
    }
}
