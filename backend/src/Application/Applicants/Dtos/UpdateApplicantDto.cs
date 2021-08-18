using FluentValidation;
using Application.Common.Models;
using Application.ElasticEnities.Dtos;

namespace Application.Applicants.Dtos
{
    public class UpdateApplicantDto : HumanDto
    {
        public string Phone { get; set; }
        public string Skype { get; set; }
        public double Experience { get; set; }
        public ElasticEnitityDto Tags { get; set; }
    }

    public class UpdateApplicantDtoValidator : AbstractValidator<UpdateApplicantDto>
    {
        public UpdateApplicantDtoValidator()
        {
            RuleFor(a => a.Phone).NotNull().NotEmpty();
            RuleFor(a => a.Skype).NotNull().NotEmpty();
            RuleFor(a => a.Experience).GreaterThanOrEqualTo(0);
        }
    }
}