using Domain.Common;

namespace Domain.Events
{
    public class CandidateStageChangedEvent : DomainEvent
    {
        public string Id { get; set; }
        public string StageId { get; set; }
        public string VacancyId { get; set; }

        public CandidateStageChangedEvent(string id, string vacancyId, string stageId)
        {
            Id = id;
            StageId = stageId;
            VacancyId = vacancyId;
        }
    }
}
