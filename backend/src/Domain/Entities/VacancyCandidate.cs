using System;
using System.Collections.Generic;
using Domain.Common;
using Domain.Common.Interfaces;

namespace Domain.Entities
{
    public class VacancyCandidate : Entity, IHasDomainEvent
    {
        public VacancyCandidate()
        {
            DomainEvents = new List<DomainEvent>();
        }

        public DateTime FirstContactDate { get; set; }
        public DateTime SecondContactDate { get; set; }
        public DateTime ThirdContactDate { get; set; }
        public int SalaryExpectation { get; set; }
        public string ContactedBy { get; set; }
        public string Comments { get; set; }
        public double Experience { get; set; }
        public string ApplicantId { get; set; }

        public Applicant Applicant { get; set; }
        public Stage Stage { get; set; }
        public ICollection<CandidateToStage> CandidateToStages { get; set; }
        public IList<DomainEvent> DomainEvents { get; set; }
    }
}