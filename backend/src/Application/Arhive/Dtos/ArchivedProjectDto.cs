using Application.Common.Models;
using System;
using System.Collections.Generic;

namespace Application.Arhive.Dtos
{
    public class ArchivedProjectDto: Dto
    {
        public string Logo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TeamInfo { get; set; }
        public DateTime CreationDate { get; set; }
        public ArchivedEntityDto ArchivedProjectData { get; set; }
        public ICollection<ArchivedVacancyShortDto> Vacancies { get; set; } = new List<ArchivedVacancyShortDto>();
    }
}
