using Application.Common.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Projects.Dtos
{
    public class ProjectDto : Dto
    {
        public string Logo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TeamInfo { get; set; }
        public string AdditionalInfo { get; set; }
        public string WebsiteLink { get; set; }
        public string CompanyId { get; set; }
    }

    public class ProjectDtoValidator : AbstractValidator<ProjectDto>
    {
        public ProjectDtoValidator()
        {
            RuleFor(_ => _.Logo).NotNull().NotEmpty();
            RuleFor(_ => _.Name).NotNull().NotEmpty();
            RuleFor(_ => _.Description).NotNull().NotEmpty();
            RuleFor(_ => _.TeamInfo).NotNull().NotEmpty();
        }
    }
}
