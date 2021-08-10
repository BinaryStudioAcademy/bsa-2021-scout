using Application.Common.Models;

namespace Application.CandidateReviews.Dtos
{
    public class CandidateReviewShortDto : Dto
    {
        public string ReviewName { get; set; }
        public int Mark { get; set; }
    }
}
