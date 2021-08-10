using Application.Common.Models;
using System;

namespace Application.Applicants.Dtos
{
    public class ApplicantDto : HumanDto
    {
        public string Phone { get; set; }
        public string Skype { get; set; }
        public double Experience { get; set; }
        public DateTime ToBeContacted { get; set; }
        public string CompanyId { get; set; }
    }
}