
namespace Application.Home.Dtos
{
    public class HotVacancySummaryDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int CandidateCount { get; set; }
        public int ProcessedCount { get; set; }
        public int SelfAppliedCount { get; set; }
        public int CandidateNewCount { get; set; }
    }
}
