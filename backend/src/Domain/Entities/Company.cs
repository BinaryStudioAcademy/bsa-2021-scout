using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Company : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
    
        public ICollection<Pool> Pools { get; private set;}
        public ICollection<Applicant> Applicants { get; private set; }
        public ICollection<Vacancy> Vacancies { get; private set; }
        public ICollection<Project> Projects { get; private set; }
        public ICollection<CompanyToUser> Recruiters { get; private set; }
    }
}