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
        public Company Company { get; set; }
        public ICollection<PoolToApplicant> ApplicantPools { get; set; }
        public ICollection<VacancyCandidate> Candidates { get; set; }
    }
}