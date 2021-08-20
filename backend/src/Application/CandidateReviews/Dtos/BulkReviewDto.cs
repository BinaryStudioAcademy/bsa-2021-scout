using System.Collections.Generic;
using Application.Common.Models;

namespace Application.CandidateReviews.Dtos
{
    public class BulkReviewDto : Dto
    {
        public string StageId { get; set; }
        public string CandidateId { get; set; }
        public string Comment { get; set; }

        // Dictionary<candidate id, mark>
        public Dictionary<string, int> Data { get; set; }
    }
}
