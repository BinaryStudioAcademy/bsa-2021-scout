using Domain.Common;
using Domain.Entities;

namespace Domain.Events
{
    public class UserCreatedEvent : DomainEvent
    {
        public UserCreatedEvent(User user)
        {
            User = user;
        }

        public User User { get; }
    }
}
