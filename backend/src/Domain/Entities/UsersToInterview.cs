using Domain.Common;

namespace Domain.Entities
{
    public class UsersToInterview: Entity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public string InterviewId { get; set; }
        public Interview Interview { get; set; }
    }
}