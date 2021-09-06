using Domain.Enums;
using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Stage : Entity
    {
        public string Name { get; set; }
        public StageType Type { get; set; }
        public int Index { get; set; }
        public bool IsReviewable { get; set; }
        public string VacancyId { get; set; }
        public string? DataJson { get; set; }

        public Vacancy Vacancy { get; set; }
        public ICollection<Action> Actions { get; set; }
        public ICollection<CandidateToStage> CandidateToStages { get; set; }
        public ICollection<ReviewToStage> ReviewToStages { get; set; }
        public ICollection<CandidateReview> Reviews { get; set; }
        public ICollection<CandidateComment> CandidateComments { get; set; }
    }
}