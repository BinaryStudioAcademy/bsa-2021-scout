using System;

namespace Application.VacancyCandidates.Dtos
{
    public class VacancyCandidateDto
    {
        public string Id { get; set; }
        public DateTime FirstContactDate { get; set; }
        public DateTime SecondContactDate { get; set; }
        public DateTime ThirdContactDate { get; set; }
        public int SalaryExpectation { get; set; }
        public string ContactedBy { get; set; }
        public string Comments { get; set; }
        public double Experience { get; set; }
        public string StageId { get; set; }
        public string ApplicantId { get; set; }
    }
}
