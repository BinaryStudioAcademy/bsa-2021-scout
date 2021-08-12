using Domain.Common;

namespace Domain.Entities
{
    public class CandidateReview : Entity
    {
        public string CandidateId { get; set; }
        public string StageId { get; set; }
        public string ReviewId { get; set; }
        public double Mark { get; set; }

        public VacancyCandidate Candidate { get; set; }
        public Stage Stage { get; set; }
        public Review Review { get; set; }
    }
}
