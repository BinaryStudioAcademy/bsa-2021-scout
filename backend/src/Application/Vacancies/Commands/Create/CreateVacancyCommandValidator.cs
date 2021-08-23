using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Commands;
using Application.Vacancies.Dtos;
using FluentValidation;

namespace Application.Vacancies.Commands.Create
{
    public class CreateVacancyCommandValidator : AbstractValidator<CreateVacancyCommand>
    {
        public CreateVacancyCommandValidator()
        {
            RuleFor(x => x.VacancyCreate).NotNull().SetValidator(new VacancyCreateDtoValidator());
        }
    }
}
