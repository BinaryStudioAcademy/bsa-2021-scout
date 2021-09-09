using Application.Applicants.Dtos;
using Application.Common.Models;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Application.Tasks.Dtos
{
    public class TaskDto : Dto
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDone { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime DoneDate { get; set; }
        public TeamMemberstDto CreatedBy { get; set; }
        public bool IsReviewed { get; set; }
        public string Note { get; set; }
        public string Company { get; set; }
        public TeamMemberstDto Applicant { get; set; }
        public ICollection<TeamMemberstDto> TeamMembers { get; set; }

    }
}