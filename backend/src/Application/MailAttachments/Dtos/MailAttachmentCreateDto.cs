using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.MailAttachments.Dtos
{
    public class MailAttachmentCreateDto
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
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
