using Application.Common.Models;

namespace Application.CandidateToStages.Dtos
{
    public class CandidateToStageRecentActivityDto : Dto
    {
        public string MoverId { get; set; }
        public string MoverName { get; set; }
        public string CandidateId { get; set; }
        public string CandidateName { get; set; }
        public string StageId { get; set; }
        public string StageName { get; set; }
    }
}
