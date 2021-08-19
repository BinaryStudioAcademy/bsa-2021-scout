using System;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class VacancyTable: Entity
    {
        public string Title { get; set; }
        public int CandidatesAmount { get; set; }
        public string ProjectId { get; set; }
        public string ResponsibleHrId { get; set; }
        public Project Project { get; set; }
        public DateTime CreationDate { get; set; }
        public User ResponsibleHr { get; set; }
        public VacancyStatus Status { get; set; }
    }
}