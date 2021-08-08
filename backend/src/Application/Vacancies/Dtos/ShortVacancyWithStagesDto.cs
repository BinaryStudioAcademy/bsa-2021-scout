using System.Collections.Generic;
using Application.Stages.Dtos;
using Application.Common.Models;

namespace Application.Vacancies.Dtos
{
    public class ShortVacancyWithStagesDto : Dto
    {
        public string Title { get; set; }
        public IEnumerable<StageWithCandidatesDto> Stages { get; set; }
    }
}
