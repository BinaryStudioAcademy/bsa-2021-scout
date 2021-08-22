using Application.Common.Models;
using Application.ElasticEnities.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Projects.Dtos
{
    public class ProjectDto : Dto
    {
        public string Logo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TeamInfo { get; set; }
        public string WebsiteLink { get; set; }
        public ElasticEnitityDto Tags { get; set; }

    }

    public class ProjectDtoValidator : AbstractValidator<ProjectDto>
    {
        public ProjectDtoValidator()
        {
            RuleFor(_ => _.Logo).NotNull().NotEmpty();
            RuleFor(_ => _.Name).NotNull().NotEmpty().Length(3,15);
            RuleFor(_ => _.Description).NotNull().NotEmpty().MinimumLength(10);
            RuleFor(_ => _.TeamInfo).NotNull().NotEmpty().MinimumLength(10);
            RuleFor(_ => _.WebsiteLink).NotNull().NotEmpty()
                .Must(websiteLink => new Regex("(https?://)?([\\da-z.-]+)\\.([a-z.]{2,6})[/\\w .-]*/?").IsMatch(websiteLink));
        }
    }
}
