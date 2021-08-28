using Domain.Entities;
using Domain.Interfaces.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Read
{
    public interface IVacancyReadRepository : IReadRepository<Vacancy>
    {
        Task<Vacancy> GetByCompanyIdAsync(string id);
        Task<IEnumerable<Vacancy>> GetEnumerableNotAppliedByApplicantId(string applicantId);
    }
}
