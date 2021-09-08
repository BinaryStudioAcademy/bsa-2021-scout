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
    public class UploadMailAttachmentFileValidator : AbstractValidator<UploadMailAttachmentFileCommand>
    {
        public UploadMailAttachmentFileValidator()
        {
            RuleFor(x => x.MailAttachmentDto).NotNull().SetValidator(new MailAttachmentCreateDtoValidator());
        }
    }
}
