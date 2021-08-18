using Domain.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    public class MailTemplate : Entity
    {
        public string Slug { get; set; }
        public string Html { get; set; }
    }
}
