using Domain.Common;
using Domain.Common.Interfaces;
using System.Collections.Generic;
using Domain.Entities.Abstractions;

namespace Domain.Entities
{
    public class User: Human, IHasDomainEvent
    {
        public User()
        {
            DomainEvents = new List<DomainEvent>();
        }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }

        public Vacancy Vacancy { get; private set; }
        public RefreshToken RefreshToken { get; private set; }
        public ICollection<UserToRole> UserRoles { get; private set; }
        public ICollection<CompanyToUser> UserCompanies { get; private set; }
        
        public IList<DomainEvent> DomainEvents { get; set; }
    }
}
