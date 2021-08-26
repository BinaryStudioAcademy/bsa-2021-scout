using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MailAttachments.Dtos
{
    public class MailAttachmentCreateDto
    {
        public string MailTemplateId { get; set; }
        public string Name { get; set; }
        public string File { get; set; }
    }
    public class MailAttachmentCreateDtoValidator : AbstractValidator<MailAttachmentCreateDto>
    {
        public MailAttachmentCreateDtoValidator()
        {
            RuleFor(_ => _.Name).NotNull().NotEmpty();
            RuleFor(_ => _.File).NotNull().NotEmpty();
        }
    }

}
