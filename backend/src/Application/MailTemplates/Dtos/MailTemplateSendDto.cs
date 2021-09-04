using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Application.MailTemplates.Dtos
{
    public class MailTemplateSendDto : Dto
    {
        public string Slug { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
        public string UserCreatedId { get; set; }
        public string UserCreated { get; set; }
        public int VisibilitySetting { get; set; }
        public DateTime DateCreation { get; set; }
        public string CompanyId { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
    }
}
