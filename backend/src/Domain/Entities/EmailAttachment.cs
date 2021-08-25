﻿using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EmailAttachment : Entity
    {
        public string MailTemplateId { get; set; }
        public string Name { get; set; }
        public string File { get; set; }

    }
}
