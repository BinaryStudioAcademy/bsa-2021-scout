using Application.Common.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Applicants.Dtos
{
    public class GetShortApplicantDto : Dto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class GetShortApplicantDtoValidator : AbstractValidator<GetShortApplicantDto>
    {
        public GetShortApplicantDtoValidator()
        {
            RuleFor(a => a.FirstName).NotNull().NotEmpty();
            RuleFor(a => a.LastName).NotNull().NotEmpty();
        }
    }
}
