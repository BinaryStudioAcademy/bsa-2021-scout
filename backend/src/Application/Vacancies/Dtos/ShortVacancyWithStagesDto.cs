using System.Collections.Generic;
using Application.Stages.Dtos;

namespace Application.Vacancies.Dtos
{
    public class ShortVacancyWithStagesDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<StageWithCandidatesDto> Stages { get; set; }
    }
}
