using System;

using Application.Common.Models;
using Application.Projects.Dtos;
using Application.Users.Dtos;
using Domain.Enums;

namespace Application.Vacancies.Dtos
{
    public class VacancyTableDto: Dto
    {
        public string Title { get; set; }
        public int CandidatesAmount { get; set; }
        public string ProjectId { get; set; }
        public string ResponsibleHrId { get; set; }
        public ProjectDto Project { get; set; }
        public DateTime CreationDate { get; set; }
        public UserDto ResponsibleHr { get; set; }

        public string TeamInfo { get; set; } 
        public VacancyStatus Status { get; set; }
    }
}
