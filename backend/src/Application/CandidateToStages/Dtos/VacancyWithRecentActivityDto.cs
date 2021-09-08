using System.Collections.Generic;

namespace Application.CandidateToStages.Dtos
{
    public class VacancyWithRecentActivityDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<CandidateToStageApplicantRecentActivityDto> Activity { get; set; }
    }
}
