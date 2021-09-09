using FluentValidation;
using Application.Common.Models;
using Application.ElasticEnities.Dtos;
using System;

namespace Application.Applicants.Dtos
{
    public class UpdateApplicantDto : HumanDto
    {
        public string Phone { get; set; }
        public double Experience { get; set; }
        public string ExperienceDescription { get; set; }
        public string Skills { get; set; }
        public string CvLink { get; set; }
        public string PhotoLink { get; set; }
        public ElasticEnitityDto Tags { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public class UpdateApplicantDtoValidator : AbstractValidator<UpdateApplicantDto>
    {
        public UpdateApplicantDtoValidator()
        {
            RuleFor(a => a.Phone).NotNull().NotEmpty();
            RuleFor(a => a.Experience).GreaterThanOrEqualTo(0);
        }
    }
}