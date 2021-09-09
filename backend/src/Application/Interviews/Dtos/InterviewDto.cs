using System;
using System.Collections.Generic;
using Application.Applicants.Dtos;
using Application.Common.Models;
using Application.Users.Dtos;
using Application.Vacancies.Dtos;
using Domain.Enums;

namespace Application.Interviews.Dtos
{
    public class InterviewDto: Dto
    {
        public string Title { get; set; }
        public string MeetingLink { get; set; }
        public MeetingSource MeetingSource { get; set; }
        public string VacancyId { get; set; }
        public VacancyDto Vacancy { get; set; }
        public DateTime Scheduled { get; set; }
        public double Duration { get; set; }
        public bool IsReviewed { get; set; }
        public InterviewType InterviewType { get; set; }
        public ICollection<UserDto> UserParticipants { get; set; }
        public string CandidateId { get; set; }
        public ApplicantDto Candidate { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}