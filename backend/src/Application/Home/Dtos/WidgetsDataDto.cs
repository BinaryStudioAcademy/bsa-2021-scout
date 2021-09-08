using System.Collections.Generic;

namespace Application.Home.Dtos
{
    public class WidgetsDataDto
    {
        public int ApplicantCount { get; set; }
        public ICollection<ShortVacancyDto> Vacancies { get; set; }
        public int ProcessedCount { get; set; }
        public int HrCount { get; set; }
    }
}
