using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Write
{
    public interface IApplicantsFromCsvWriteRepository
    {
        Task<ICollection<Applicant>> CreateRangeAsync(ICollection<Applicant> applicants);
    }
}
