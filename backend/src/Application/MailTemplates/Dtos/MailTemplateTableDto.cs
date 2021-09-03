using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MailTemplates.Dtos
{
    public class MailTemplateTableDto : Dto
    {
        public string Title { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
        public int VisibilitySetting { get; set; }
        public string UserCreated { get; set; }
        public DateTime DateCreation { get; set; }
        public int AttachmentsCount { get; set; }
    }
}
