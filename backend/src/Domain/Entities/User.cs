﻿using Domain.Common;
using Domain.Common.Interfaces;
using System.Collections.Generic;
using Domain.Entities.Abstractions;

namespace Domain.Entities
{
    public class User : Human, IHasDomainEvent
    {
        public User()
        {
            DomainEvents = new List<DomainEvent>();
        }

        public string Password { get; set; }
        public string PasswordSalt { get; set; }

        public ICollection<Vacancy> Vacancies { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
        public ICollection<UserToRole> UserRoles { get; set; }
        public ICollection<CompanyToUser> UserCompanies { get; set; }

        public IList<DomainEvent> DomainEvents { get; set; }
    }
}
