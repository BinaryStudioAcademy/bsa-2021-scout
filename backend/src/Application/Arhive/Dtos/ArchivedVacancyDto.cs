using Application.Common.Models;
using Application.Users.Dtos;
using System;

namespace Application.Arhive.Dtos
{
    public class ArchivedVacancyDto: Dto
    {
        public string Title { get; set; }
        public string ProjectName { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public UserDto ResponsibleHr { get; set; }
        public ArchivedEntityDto ArchivedVacancyData { get; set; }
        public bool IsProjectArchived { get; set; }
    }
}
