using Application.Common.Models;
using Application.MailAttachments.Dtos;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Application.MailTemplates.Dtos
{
    public class MailTemplateDto : Dto
    {
        public string Slug { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
        public string UserCreatedId { get; set; }
        public string UserCreated { get; set; }
        public int VisibilitySetting { get; set; }
        public DateTime DateCreation { get; set; }
        public string CompanyId { get; set; }
        public ICollection<MailAttachmentDto> MailAttachments { get; set; }
    }
}
