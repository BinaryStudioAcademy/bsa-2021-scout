using Application.Common.Models;
using System;
using FluentValidation;

namespace Application.Applicants.Dtos
{
    public class CreateApplicantDto : HumanDto
    {
        public string Phone { get; set; }
        public string Skype { get; set; }
        public double Experience { get; set; }
        public DateTime ToBeContacted { get; set; }
        public string CompanyId { get; set; }
    }

    public class CreateApplicantDtoValidator : AbstractValidator<CreateApplicantDto>
    {
        public CreateApplicantDtoValidator()
        {
            RuleFor(a => a.Phone).NotNull().NotEmpty();
            RuleFor(a => a.Skype).NotNull().NotEmpty();
            RuleFor(a => a.Experience).GreaterThanOrEqualTo(0);
            RuleFor(a => a.CompanyId).NotNull().NotEmpty();
        }
    }
}