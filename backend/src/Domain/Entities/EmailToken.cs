
using Domain.Common;

namespace Domain.Entities
{
    public class EmailToken : Entity
    {
        public string UserId { get; set; }

        public string Token { get; set; }

        public User User { get; set; }

    }
}
