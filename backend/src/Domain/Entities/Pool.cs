using Domain.Enums;
using Domain.Common;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Pool : Entity
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedById { get; set; }
        public string Description { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }
        public User CreatedBy { get; set; }
        public ICollection<PoolToApplicant> PoolApplicants { get; set; } 
    }
}