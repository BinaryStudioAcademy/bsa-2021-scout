using System;
using Application.Common.Models;

namespace Application.Applicants.Dtos
{
    public class ApplicantDto : Dto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Skype { get; set; }
        public double Experience { get; set; }
        public DateTime ToBeContacted { get; set; }
        public string CompanyId { get; set; }
    }
}
