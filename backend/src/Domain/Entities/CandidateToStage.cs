using System;
using Domain.Common;

namespace Domain.Entities
{
    public class CandidateToStage : Entity
    {
        public CandidateToStage()
        {
            DateAdded = DateTime.UtcNow;
        }

        public string CandidateId { get; set; }
        public string StageId { get; set; }
        public string MoverId { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? DateRemoved { get; set; }

        public VacancyCandidate Candidate;
        public Stage Stage;
        public User Mover;
    }
}
