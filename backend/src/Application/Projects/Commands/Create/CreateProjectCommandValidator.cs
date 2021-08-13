using Application.Projects.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Projects.Commands.Create
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(_ => _.Project).NotNull();
            RuleFor(_ => _.Project).SetValidator(new ProjectDtoValidator());
        }
    }
}
