using System;
using Application.Common.Models;

namespace Application.VacancyCandidates.Dtos
{
    public class VacancyCandidateDto : Dto
    {
        public DateTime FirstContactDate { get; set; }
        public DateTime SecondContactDate { get; set; }
        public DateTime ThirdContactDate { get; set; }
        public int SalaryExpectation { get; set; }
        public string ContactedBy { get; set; }
        public string Comments { get; set; }
        public double Experience { get; set; }
        public string ApplicantId { get; set; }
    }
}
