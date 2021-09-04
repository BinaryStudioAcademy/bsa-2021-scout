using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MailAttachments.Dtos
{
    public class MailAttachmentDto : Dto
    {
        public string MailTemplateId { get; set; }
        public string Name { get; set; }
        public string File { get; set; }
    }
}
