
namespace Domain.Entities.HomeData
{
    public class HotVacancySummary
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string ProjectName { get; set; }
        public int CurrentStageIndex { get; set; }
        public int LastStageIndex { get; set; }
        public string HrWhoAddedId { get; set; }
    }
}
