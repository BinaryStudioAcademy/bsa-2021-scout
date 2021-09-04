using Application.Common.Models;
using Application.Users.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Applicants.Dtos
{
    public class ApplicantCsvGetDto : HumanDto
    {
        public string Phone { get; set; }
        public string LinkedInUrl { get; set; }
        public double Experience { get; set; }
        public string ExperienceDescription { get; set; }
        public string Skills { get; set; }
        public bool HasCv { get; set; }
        public bool IsSelfApplied { get; set; }
        public DateTime CreationDate { get; set; }
        public UserDto User { get; set; }
    }
}
