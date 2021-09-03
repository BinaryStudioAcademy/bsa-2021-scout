using System;
using Domain.Common;
using Domain.Enums;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Vacancy : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public VacancyStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DateOfOpening { get; set; }
        public DateTime ModificationDate { get; set; }
        public bool IsRemote { get; set; }
        public bool IsHot { get; set; }
        public int SalaryFrom { get; set; }
        public int SalaryTo { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime PlannedCompletionDate { get; set; }
        public Tier TierFrom { get; set; }
        public Tier TierTo { get; set; }
        public string Sources { get; set; }
        public string CompanyId { get; set; }
        public string ResponsibleHrId { get; set; }
        public string ProjectId { get; set; }

        public Company Company { get; set; }
        public User ResponsibleHr { get; set; }
        public Project Project { get; set; }
        public ICollection<Stage> Stages { get; set; }
        //public ICollection<TagToVacancy> VacancyTags { get; set; }
        public ICollection<ApplyToken> ApplyTokens { get; set; }
    }
}