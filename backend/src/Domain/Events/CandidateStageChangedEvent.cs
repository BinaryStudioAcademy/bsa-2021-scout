using Domain.Common;

namespace Domain.Events
{
    public class CandidateStageChangedEvent : DomainEvent
    {
        public string Id { get; set; }
        public string StageId { get; set; }

        public CandidateStageChangedEvent(string id, string stageId)
        {
            Id = id;
            StageId = stageId;
        }
    }
}
