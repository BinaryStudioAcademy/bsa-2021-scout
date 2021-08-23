using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Vacancies.Dtos;
using FluentValidation;

namespace Application.Vacancies.Commands.Edit
{
    public class EditVacancyCommandValidator : AbstractValidator<EditVacancyCommand>
    {
        public EditVacancyCommandValidator()
        {
            RuleFor(x => x.VacancyUpdate).NotNull().SetValidator(new VacancyUpdateDtoValidator());
        }
    }
}
