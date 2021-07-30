using Domain.Common;

namespace Domain.Entities
{
    public class CompanyToUser : Entity
    {
        public string CompanyId { get; set; }
        public string UserId { get; set; }

        public Company Company { get; set; }
        public User User { get; set; }
    }
}