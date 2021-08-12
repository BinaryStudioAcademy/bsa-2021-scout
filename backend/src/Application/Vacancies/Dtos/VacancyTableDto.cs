using System;
using Application.Common.Models;
using Application.Users.Dtos;
using Domain.Enums;

namespace Application.Vacancies.Dtos
{
    public class VacancyTableDto: Dto
    {
        public string Title { get; set; }
        public int CurrentApplicantsAmount { get; set; }
        public int RequiredCandidatesAmount { get; set; }
        public string ProjectId { get; set; }
        public string ResponsibleHrId { get; set; }
        public DateTime CreationDate { get; set; }
        public UserDto ResponsibleHr { get; set; }
        public string Department { get; set; } 
        public VacancyStatus Status { get; set; }
    }
}