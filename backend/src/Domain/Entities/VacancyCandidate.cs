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
            DateAdded = DateTime.UtcNow;
            DomainEvents = new List<DomainEvent>();
        }

        public DateTime? FirstContactDate { get; set; }
        public DateTime? SecondContactDate { get; set; }
        public DateTime? ThirdContactDate { get; set; }
        public int SalaryExpectation { get; set; }
        public string ContactedById { get; set; }
        public string Comments { get; set; }
        public double Experience { get; set; }
        public string ApplicantId { get; set; }
        public string HrWhoAddedId { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsSelfApplied { get; set; }
        public bool IsViewed { get; set; }

        public Applicant Applicant { get; set; }
        public User HrWhoAdded { get; set; }
        public ICollection<CandidateToStage> CandidateToStages { get; set; }
        public ICollection<CandidateReview> Reviews { get; set; }
        public ICollection<CandidateComment> CandidateComments { get; set; }
        public IList<DomainEvent> DomainEvents { get; set; }
    }
}