using Application.Common.Models;

namespace Application.Arhive.Dtos
{
    public class ArchivedVacancyShortDto: Dto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
    }
}
