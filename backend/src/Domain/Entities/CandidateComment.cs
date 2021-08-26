using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class CandidateComment : Entity
    {
        public string StageId { get; set; }
        public string CandidateId { get; set; }
        [MaxLength(1000)] public string Text { get; set; }

        public Stage Stage { get; set; }
        public VacancyCandidate Candidate { get; set; }
    }
}
