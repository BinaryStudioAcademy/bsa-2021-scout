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
    public class VacancyUpdateDto
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
        public ElasticEnitityDto Tags { get; set; }
        public ICollection<StageUpdateDto> Stages { get; set; }

    }
    public class VacancyUpdateDtoValidator : AbstractValidator<VacancyUpdateDto>
    {
        public VacancyUpdateDtoValidator()
        {
            RuleFor(_ => _.Title).NotNull().NotEmpty();
            RuleFor(_ => _.Description).NotNull().NotEmpty();
            RuleFor(_ => _.Requirements).NotNull().NotEmpty();
            RuleFor(_ => _.ProjectId).NotNull().NotEmpty();
            RuleFor(_ => _.SalaryFrom).NotNull().GreaterThanOrEqualTo(0);
            RuleFor(_ => _.SalaryTo).NotNull().GreaterThanOrEqualTo(_ => _.SalaryFrom);
            RuleFor(_ => _.TierFrom).NotNull().NotEmpty();
            RuleFor(_ => (int)_.TierTo).NotNull().NotEmpty().GreaterThanOrEqualTo(_ => (int)_.TierFrom);
            RuleFor(_ => _.Sources).NotNull().NotEmpty();
        }
    }
}
