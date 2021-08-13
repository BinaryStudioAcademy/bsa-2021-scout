using Application.Common.Commands;
using Application.Projects.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Projects.CommandQuery.Update
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateEntityCommand<ProjectDto>>
    {
        public UpdateProjectCommandValidator()
        {
            RuleFor(_ => _.Entity).NotNull();
            RuleFor(_ => _.Entity).SetValidator(new ProjectDtoValidator());
        }
    }
}
