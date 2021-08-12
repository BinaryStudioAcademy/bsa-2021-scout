using Application.Common.Models;

namespace Application.MailTemplates.Dtos
{
    public class MailTemplateDto : Dto
    {
        public string Slug { get; set; }
        public string Html { get; set; }
    }
}
