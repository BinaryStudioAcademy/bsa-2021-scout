using Domain.Enums;
using Domain.Common;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ToDoTask : Entity
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public bool IsDone { get; set; }
        public bool IsReviewed { get; set; }
        public string CreatedById { get; set; }
        public string Note { get; set; }
        public string CompanyId { get; set; }
        public string ApplicantId { get; set; }
        public Applicant Applicant { get; set; }
        public Company Company { get; set; }
        public User CreatedBy { get; set; }
        public ICollection<UserToTask> TeamMembers { get; set; }
        

    }
}