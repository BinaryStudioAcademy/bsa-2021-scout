using System;
using Application.Applicants.Dtos;
using Application.Common.Models;

namespace Application.VacancyCandidates.Dtos
{
    public class VacancyCandidateWithApplicantDto : Dto
    {
        public DateTime FirstContactDate { get; set; }
        public DateTime SecondContactDate { get; set; }
        public DateTime ThirdContactDate { get; set; }
        public int SalaryExpectation { get; set; }
        public string ContactedBy { get; set; }
        public string Comments { get; set; }
        public double Experience { get; set; }
        public string StageId { get; set; }
        public ApplicantDto Applicant { get; set; }
    }
}
