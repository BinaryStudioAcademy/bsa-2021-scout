using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Review : Entity
    {
        public string Name { get; set; }

        public ICollection<CandidateReview> CandidateReviews { get; set; }
        public ICollection<ReviewToStage> ReviewToStages { get; set; }
    }
}
