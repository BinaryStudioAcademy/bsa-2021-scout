using Domain.Common;

namespace Domain.Entities.Abstractions
{
    public abstract class Job : Entity
    {
        public string TriggerId { get; set; }

        public User Trigger { get; set; }
    }
}
