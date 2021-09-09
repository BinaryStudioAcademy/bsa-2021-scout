using System;
using System.Collections.Generic;
using Application.Common.Models;
using Domain.Common;

namespace Application.VacancyCandidates.Dtos
{
    public class VacancyCandidateDto : Dto
    {
        public DateTime? FirstContactDate { get; set; }
        public DateTime? SecondContactDate { get; set; }
        public DateTime? ThirdContactDate { get; set; }
        public int SalaryExpectation { get; set; }
        public string ContactedById { get; set; }
        public string Comments { get; set; }
        public double Experience { get; set; }
        public string ApplicantId { get; set; }
        public string PhotoLink { get; set; }
        public IList<DomainEvent> DomainEvents { get; set; }
    }
}
