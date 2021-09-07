using Domain.Enums;
using Domain.Common;

namespace Domain.Entities
{
    public class Action : Entity
    {
        public string Name { get; set; }
        public ActionType ActionType { get; set; }
        public string StageId { get; set; }
        public StageChangeEventType StageChangeEventType { get; set; }

        public Stage Stage { get; set; }
    }
}