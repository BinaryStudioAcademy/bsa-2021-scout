using Domain.Common;

namespace Domain.Entities
{
    public class UserToTask
    {
        public string UserId { get; set; }
        public string ToDoTaskId { get; set; }

        public User User { get; set; }
        public ToDoTask Task { get; set; }
        
    }
}