using System;
using System.Collections.Generic;
using Application.Common.Models;
using FluentValidation;

namespace Application.Tasks.Dtos
{
    public class UpdateTaskDto : Dto
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public bool IsDone { get; set; }
        public DateTime DueDate { get; set; }
        public string ApplicantId { get; set; }
        public List<string> UsersIds { get; set; }
        public bool IsReviewed { get; set; }
    }

    public class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskDtoValidator()
        {
            RuleFor(a => a.Name).NotNull().NotEmpty();
            RuleFor(a => a.DueDate).NotNull().NotEmpty();
            RuleFor(a => a.ApplicantId).NotNull().NotEmpty();            

        }
    }
}