using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.ElasticEnities.Dtos;
using Application.Stages.Dtos;
using Domain.Enums;
using FluentValidation;

namespace Application.Vacancies.Dtos
{
    public class VacancyDto:Dto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Requirements { get; set; }
        public string ProjectId { get; set; }
        public int SalaryFrom { get; set; }
        public int SalaryTo { get; set; }
        public Tier TierFrom { get; set; }
        public Tier TierTo { get; set; }
        public string Sources { get; set; }
        public bool IsHot { get; set; }
        public bool IsRemote { get; set; }
        public string CompanyId { get; set; }
        public string ResponsibleHrId { get; set; }
        public VacancyStatus Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DateOfOpening { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public DateTime PlannedCompletionDate { get; set; }
        public ICollection<StageDto> Stages { get; set; }
        public ElasticEnitityDto Tags { get; set; }
    }
    public class VacancyDtoValidator : AbstractValidator<VacancyDto>
    {
        public VacancyDtoValidator()
        {
            RuleFor(_ => _.Title).NotNull().NotEmpty();
            RuleFor(_ => _.Description).NotNull().NotEmpty();
        }
    }
}
