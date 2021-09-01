using Application.Common.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Applicants.Dtos
{
    public class GetApplicantDto : HumanDto
    {
        public string Phone { get; set; }
        public double Experience { get; set; }
    }

    public class GetApplicantDtoValidator : AbstractValidator<GetApplicantDto>
    {
        public GetApplicantDtoValidator()
        {
            RuleFor(a => a.Phone).NotNull().NotEmpty();
            RuleFor(a => a.Experience).GreaterThanOrEqualTo(0);
        }
    }
}
