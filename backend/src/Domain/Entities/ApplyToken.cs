using Domain.Common;
using System;

namespace Domain.Entities
{
    public class ApplyToken : Entity
    {
        private const int DaysToExpire = 1;
        public ApplyToken()
        {
            Expires = DateTime.UtcNow.AddDays(DaysToExpire);
        }
        public string Token { get; set; }
        public DateTime Expires { get; private set; }
        public string Email { get; set; }
        public string VacancyId { get; set; }
        public bool IsActive { get; set; }

        public Vacancy Vacancy { get; set; }

    }
}
