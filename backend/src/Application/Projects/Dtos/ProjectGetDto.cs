using Application.ElasticEnities.Dtos;
using Application.Vacancies.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Projects.Dtos
{
    public class ProjectGetDto : ProjectDto
    {
        public string CompanyId { get; set; }
        public DateTime CreationDate { get; set; }
        public ICollection<VacancyTableDto> Vacancies { get; set; }
    }
    public class ProjectGetDtoValidator : AbstractValidator<ProjectGetDto>
    {
        public ProjectGetDtoValidator()
        {
            RuleFor(_ => _).SetValidator(new ProjectDtoValidator());
        }
    }
}
