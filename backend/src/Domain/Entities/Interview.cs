using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Common;
using Domain.Enums;

namespace Domain.Entities
{
    public class Interview: Entity
    {
        public string Title { get; set; }
        public string MeetingLink { get; set; }
        public MeetingSource MeetingSource { get; set; }
        public string CompanyId { get; set; }
        public string VacancyId { get; set; }
        public Vacancy Vacancy { get; set; }
        public DateTime Scheduled { get; set; }
        public double Duration { get; set; }
        public bool IsReviewed { get; set; }
        public InterviewType InterviewType { get; set; }
        public ICollection<UsersToInterview> UserParticipants { get; set; }
        public string CandidateId { get; set; }
        public Applicant Candidate { get; set; }
        [MaxLength(1000)] public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}