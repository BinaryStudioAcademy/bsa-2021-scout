using System.Collections.Generic;

namespace Application.CandidateToStages.Dtos
{
    public class RecentActivityInfoDto
    {
        public bool IsEnd { get; set; }
        public IEnumerable<CandidateToStageRecentActivityDto> Data { get; set; }
    }
}
