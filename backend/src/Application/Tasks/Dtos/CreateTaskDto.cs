using Application.Common.Models;
using System;
using FluentValidation;
using System.Collections.Generic;

namespace Application.Tasks.Dtos
{
    public class CreateTaskDto : Dto
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public DateTime DueDate { get; set; }
        public string ApplicantId { get; set; }
        public List<string> UsersIds { get; set; }
        public bool IsReviewed { get; set; }
    }

    public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
    {
        public CreateTaskDtoValidator()
        {
            RuleFor(a => a.Name).NotNull().NotEmpty();
            RuleFor(a => a.DueDate).NotNull().NotEmpty();
            RuleFor(a => a.ApplicantId).NotNull().NotEmpty();
        }
    }
}