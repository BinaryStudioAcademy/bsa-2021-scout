using System.Collections.Generic;
namespace Domain.Entities
{
    public class Applicant:Human
    {
        public IList<string> Tags { get; set; }
        public int WorkExperienceInYears { get; set; }
    }
}