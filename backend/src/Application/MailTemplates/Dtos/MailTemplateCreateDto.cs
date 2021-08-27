using Application.Common.Models;
using Application.MailAttachments.Dtos;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MailTemplates.Dtos
{
    public class MailTemplateCreateDto
    {
        public string Slug { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
        public int VisibilitySetting { get; set; }
        public ICollection<MailAttachmentCreateDto> MailAttachments { get; set; }
    }

    public class MailTemplateCreateDtoValidator : AbstractValidator<MailTemplateCreateDto>
    {
        public MailTemplateCreateDtoValidator()
        {
            RuleFor(_ => _.Slug).NotNull().NotEmpty();
            RuleFor(_ => _.Subject).NotNull().NotEmpty();
            RuleFor(_ => _.Html).NotNull().NotEmpty();
            RuleFor(_ => _.VisibilitySetting).GreaterThanOrEqualTo(0).LessThan((int)VisibilitySetting.OnlyForTheCreator); ;
        }
    }
}
