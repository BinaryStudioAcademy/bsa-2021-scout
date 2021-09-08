using Domain.Common;
using Domain.Enums;

namespace Domain.Events
{
    public class CandidateStageChangedEvent : DomainEvent
    {
        public string Id { get; set; }
        public string StageId { get; set; }
        public string VacancyId { get; set; }
        public StageChangeEventType EventType { get; set; }

        public CandidateStageChangedEvent(string id, string vacancyId, string stageId, StageChangeEventType eventType)
        {
            Id = id;
            StageId = stageId;
            VacancyId = vacancyId;
            EventType = eventType;
        }
    }
}
