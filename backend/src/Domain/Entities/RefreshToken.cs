using System;
using Domain.Common;

namespace Domain.Entities
{
    public class RefreshToken : Entity
    {
        private const int DaysToExpire = 5;
        public RefreshToken()
        {
            Expires = DateTime.UtcNow.AddDays(DaysToExpire);
        }
        public string Token { get; set; }
        public DateTime Expires { get; private set; }
        public string UserId { get; set; }
        public bool IsActive => DateTime.UtcNow <= Expires;

        public User User { get; set; }
    }
}