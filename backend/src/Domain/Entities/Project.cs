using System;
using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Project : Entity
    {
        public string Logo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string TeamInfo { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime CreationDate { get; set; }
        public string WebsiteLink { get; set; }
        public string CompanyId { get; set; }

        public Company Company { get; set; }
        public ICollection<Vacancy> Vacancies { get; set; }
    }
}