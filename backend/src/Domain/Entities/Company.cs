using Domain.Common;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Company : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
    
        public ICollection<Pool> Pools { get; set;}
        public ICollection<Applicant> Applicants { get; set; }
        public ICollection<Vacancy> Vacancies { get; set; }
        public ICollection<Project> Projects { get; set; }
        public ICollection<ToDoTask> Tasks { get; set; }
        public ICollection<User> Recruiters { get; set; }
    }
}