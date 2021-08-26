using Application.MailAttachments.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MailAttachments.Commands.Update
{
    public class UpdateMailAttachmentCommandValidator : AbstractValidator<UpdateMailAttachmentCommand>
    {
        public UpdateMailAttachmentCommandValidator()
        {
            RuleFor(x => x.MailAttachmentDto).NotNull().SetValidator(new MailAttachmentUpdateDtoValidator());
        }
    }
}
