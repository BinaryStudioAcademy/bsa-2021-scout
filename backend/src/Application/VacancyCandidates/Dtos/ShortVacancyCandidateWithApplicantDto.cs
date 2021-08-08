using System;
using Application.Applicants.Dtos;
using Application.Common.Models;

namespace Application.VacancyCandidates.Dtos
{
    public class ShortVacancyCandidateWithApplicantDto : Dto
    {
        public int? AverageMark { get; set; }
        public DateTime DateAdded { get; set; }
        public ApplicantDto Applicant { get; set; }
    }
}
