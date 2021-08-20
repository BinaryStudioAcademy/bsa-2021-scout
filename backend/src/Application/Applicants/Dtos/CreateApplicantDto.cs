using FluentValidation;
using Application.Common.Models;
using Application.ElasticEnities.Dtos;

namespace Application.Applicants.Dtos
{
    public class CreateApplicantDto : HumanDto
    {
        public string Phone { get; set; }
        public string Skype { get; set; }
        public string LinkedInUrl { get; set; }
        public double Experience { get; set; }
        public ElasticEnitityDto Tags { get; set; }
    }

    public class CreateApplicantDtoValidator : AbstractValidator<CreateApplicantDto>
    {
        public CreateApplicantDtoValidator()
        {
            RuleFor(a => a.Experience).GreaterThanOrEqualTo(0);
        }
    }
}