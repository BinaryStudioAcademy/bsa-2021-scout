using Application.Common.Models;

namespace Application.Home.Dtos
{
    public class ShortVacancyDto: Dto
    {
        public string Title { get; set; }
        public bool IsHot { get; set; }
    }
}
