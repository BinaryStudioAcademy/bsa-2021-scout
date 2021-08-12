using System;
using System.Collections.Generic;
using Domain.Entities.Abstractions;

namespace Domain.Entities
{
    public class Applicant : Human
    {
        public string Phone { get; set; }
        public string Skype { get; set; }
        public double Experience { get; set; }
        public DateTime ToBeContacted { get; set; }
        public string CompanyId { get; set; }
        // TODO: add company read repository and remove nullability
        public Company? Company { get; set; }
        public FileInfo? CvFileInfo { get; set; }
        public ICollection<PoolToApplicant> ApplicantPools { get; set; } = new List<PoolToApplicant>();
        public ICollection<VacancyCandidate> Candidates { get; set; } = new List<VacancyCandidate>();
    }
}