using Domain.Common;

namespace Domain.Entities
{
    public class MailTemplate : Entity
    {
        public string Slug { get; set; }
        public string Html { get; set; }
    }
}
