using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces.Read
{
    public interface IApplicantReadRepository
    {
        Task<FileInfo> GetCvFileInfoAsync(string applicantId);
        Task<IEnumerable<ApplicantVacancyInfo>> GetApplicantVacancyInfoListAsync(string applicantId);
        Task<IEnumerable<Applicant>> GetCompanyApplicants();
        Task<Applicant> GetByIdAsync(string applicantId);
        Task<IEnumerable<(Applicant, bool)>> GetApplicantsWithAppliedMark(string vacancyId);
        Task<Applicant> GetByCompanyIdAsync(string id);
    }
}
