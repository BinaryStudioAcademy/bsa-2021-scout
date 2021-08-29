using Domain.Common;
using Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class MailTemplate : Entity
    {
        public string Slug { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
        public string UserCreatedId { get; set; }
        public string UserCreated { get; set; }
        public VisibilitySetting VisibilitySetting { get; set;}
        public DateTime DateCreation { get; set; }
        public string CompanyId { get; set; }
        public ICollection<MailAttachment> MailAttachments { get; set; }

    }
}
