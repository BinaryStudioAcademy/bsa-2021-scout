using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Applicants.Dtos
{
    public class GetApplicantCsvDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LinkedInUrl { get; set; }
        public double Experience { get; set; }
        public bool IsValid { get; set; }
        public bool IsExist { get; set; }
        public bool IsRepeat { get; set; }
    }
}
