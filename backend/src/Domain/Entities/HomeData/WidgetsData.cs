using System.Collections.Generic;

namespace Domain.Entities.HomeData
{
    public class WidgetsData
    {
        public int ApplicantCount { get; set; }
        public ICollection<Vacancy> Vacancies { get; set; }
        public int ProcessedCount { get; set; }
        public int HrCount { get; set; }
    }
}
