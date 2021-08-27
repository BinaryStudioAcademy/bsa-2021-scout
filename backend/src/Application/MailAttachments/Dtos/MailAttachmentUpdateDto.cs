using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MailAttachments.Dtos
{
    public class MailAttachmentUpdateDto
    {
        public string? Id { get; set; }
        public string MailTemplateId { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public IFormFile File { get; set; }
    }
    public class MailAttachmentUpdateDtoValidator : AbstractValidator<MailAttachmentUpdateDto>
    {
        public MailAttachmentUpdateDtoValidator()
        {
            RuleFor(_ => _.Name).NotNull().NotEmpty();
            RuleFor(_ => _.File).NotNull().NotEmpty();
        }
    }
}
