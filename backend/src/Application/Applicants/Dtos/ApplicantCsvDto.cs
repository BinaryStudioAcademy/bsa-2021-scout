using CsvHelper.Configuration;
using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace Application.Applicants.Dtos
{
    public class ApplicantCsvDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LinkedInUrl { get; set; }
        public double Experience { get; set; }
    }

    public class ApplicantCsvDtoValidator : AbstractValidator<ApplicantCsvDto>
    {
        public ApplicantCsvDtoValidator()
        {
            RuleFor(_ => _.FirstName).NotNull().NotEmpty();
            RuleFor(_ => _.LastName).NotNull().NotEmpty();
            RuleFor(_ => _.BirthDate).NotNull().NotEmpty();
            RuleFor(_ => _.Email).NotNull().EmailAddress();
            RuleFor(_ => _.Phone).NotNull().NotEmpty();
            RuleFor(_ => _.LinkedInUrl)
                .Must(LinkedInUrl => 
                new Regex(@"^https:\/\/www.linkedin.com\/[a-z0-9\-]+").IsMatch(LinkedInUrl) || LinkedInUrl=="");
            RuleFor(_ => _.Experience).GreaterThanOrEqualTo(0);
        }
    }

    public class ApplicantCsvDtoClassMap : ClassMap<ApplicantCsvDto>
    {
        public ApplicantCsvDtoClassMap()
        {
            Map(a => a.FirstName).Name("FirstName");
            Map(a => a.LastName).Name("LastName");
            Map(a => a.BirthDate).Name("BirthDate");
            Map(a => a.Email).Name("Email");
            Map(a => a.Phone).Name("Phone");
            Map(a => a.LinkedInUrl).Name("LinkedInUrl");
            Map(a => a.Experience).Name("Experience");
        }
    }
}
