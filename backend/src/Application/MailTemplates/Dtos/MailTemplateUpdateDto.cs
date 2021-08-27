using Application.Common.Models;
using Application.MailAttachments.Dtos;
using Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MailTemplates.Dtos
{
    public class MailTemplateUpdateDto : Dto
    {
        public string Slug { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
        public int VisibilitySetting { get; set; }
        public ICollection<MailAttachmentUpdateDto> MailAttachments { get; set; }
    }

    public class MailTemplateUpdateDtoValidator : AbstractValidator<MailTemplateUpdateDto>
    {
        public MailTemplateUpdateDtoValidator()
        {
            RuleFor(_ => _.Id).NotNull().NotEmpty();
            RuleFor(_ => _.Slug).NotNull().NotEmpty();
            RuleFor(_ => _.Subject).NotNull().NotEmpty();
            RuleFor(_ => _.Html).NotNull().NotEmpty();
            RuleFor(_ => _.VisibilitySetting).GreaterThanOrEqualTo(0).LessThan((int)VisibilitySetting.OnlyForTheCreator); ;
        }
    }
}
