using Application.Common.Models;

namespace Application.CandidateReviews.Dtos
{
    public class CandidateReviewShortDto : Dto
    {
        public string StageName { get; set; }
        public string ReviewName { get; set; }
    }
}
