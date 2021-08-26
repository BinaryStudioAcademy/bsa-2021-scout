using Application.MailAttachments.Dtos;
using Application.MailTemplates.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MailTemplates.Commands.Create
{
    public class CreateMailAttachmentCommandValidator : AbstractValidator<CreateMailAttachmentCommand>
    {
        public CreateMailAttachmentCommandValidator()
        {
            RuleFor(x => x.MailAttachmentDto).NotNull().SetValidator(new MailAttachmentCreateDtoValidator());
        }
    }
}
