using Application.MailTemplates.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MailTemplates.Commands.Update
{
    public class UpdateMailTemplateCommandValidator : AbstractValidator<UpdateMailTemplateCommand>
    {
        public UpdateMailTemplateCommandValidator()
        {
            RuleFor(x => x.MailTemplateDto).NotNull().SetValidator(new MailTemplateUpdateDtoValidator());
        }
    }
}
