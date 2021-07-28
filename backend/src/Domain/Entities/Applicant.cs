using System;
using System.Collections.Generic;
using Domain.Entities.Abstractions;

namespace Domain.Entities
{
    public class Applicant : Human
    {
        public string Phone { get; set; }
        public string Skype { get; set; }
        public string Experience { get; set; }
        public DateTime ToBeContacted { get; set; }
        public string CompanyId { get; set; }

        public Company Company { get; private set; }
        public ICollection<PoolToApplicant> ApplicantPools { get; private set; }
        public ICollection<VacancyCandidate> Candidates { get; private set; }
    }
}