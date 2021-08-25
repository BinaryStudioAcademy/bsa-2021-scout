using Application.Common.Models;
using MongoDB.Bson;

namespace Application.MailTemplates.Dtos
{
    public class MailTemplateDto : Dto
    {
        public string Slug { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
        public string UserCreatedId { get; set; }
        public int VisibilitySetting { get; set; }
        public BsonDateTime DateCreation { get; set; }
        public string CompanyId { get; set; }
    }
}
