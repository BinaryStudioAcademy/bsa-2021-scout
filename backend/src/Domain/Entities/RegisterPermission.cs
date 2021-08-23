using Domain.Common;
using System;

namespace Domain.Entities
{
    public class RegisterPermission : Entity
    {
        private const int DaysToExpire = 5;
        public RegisterPermission()
        {
            Expires = DateTime.UtcNow.AddDays(DaysToExpire);
        }
        public string Email { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }

        public DateTime Expires { get; private set; }
        public bool IsActive => DateTime.UtcNow <= Expires;
    }
}
