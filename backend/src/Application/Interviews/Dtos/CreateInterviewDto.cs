using System;
using System.Collections.Generic;
using Application.Applicants.Dtos;
using Application.Common.Models;
using Application.Users.Dtos;
using Application.Vacancies.Dtos;
using Domain.Enums;
using FluentValidation;

namespace Application.Interviews.Dtos
{
    public class CreateInterviewDto
    {
        public string Title { get; set; }
        public string MeetingLink { get; set; }
        public MeetingSource MeetingSource { get; set; }
        public string VacancyId { get; set; }
        public DateTime Scheduled { get; set; }
        public double Duration { get; set; }
        public InterviewType InterviewType { get; set; }
        public ICollection<string> UserParticipants { get; set; }
        public string CandidateId { get; set; }
        public bool IsReviewed { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
    }
      public class InterviewDtoValidator : AbstractValidator<CreateInterviewDto>
    {
        public InterviewDtoValidator()
        {
            RuleFor(_ => _.Title).NotNull().NotEmpty();
            RuleFor(_ => _.Scheduled).NotNull().NotEmpty();
            RuleFor(_ => _.Duration).NotNull().NotEmpty();
        }
    }
}