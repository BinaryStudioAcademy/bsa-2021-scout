using System.Threading.Tasks;
using Domain.Interfaces.Abstractions;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Interfaces.Read
{
    public interface IApplicantsReadRepository : IReadRepository<Applicant>
    {
        Task<IEnumerable<ApplicantVacancyInfo>> GetApplicantVacancyInfoListAsync(string applicantId);
        Task<IEnumerable<Applicant>> GetCompanyApplicants();
    }
}