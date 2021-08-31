using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Write
{
    public interface IApplicantsFromCsvWriteRepository
    {
        Task<IEnumerable<Applicant>> CreateRangeAsync(IEnumerable<Applicant> applicants);
    }
}
