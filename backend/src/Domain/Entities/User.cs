using Domain.Common;
using Domain.Common.Interfaces;
using System.Collections.Generic;
using Domain.Entities.Abstractions;
using System;

namespace Domain.Entities
{
    public class User : Human, IHasDomainEvent
    {
        public User()
        {
            DomainEvents = new List<DomainEvent>();
            CreationDate = DateTime.UtcNow;
        }

        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreationDate { get; set; }

        public string CompanyId { get; set; }
        public Company Company { get; set; }

        public ICollection<Vacancy> Vacancies { get; set; }
        public ICollection<VacancyCandidate> AddedCandidates { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
        public ICollection<UserToRole> UserRoles { get; set; }

        public IList<DomainEvent> DomainEvents { get; set; }
    }
}
