using CsvHelper.Configuration;
using FluentValidation;
using System;

namespace Application.Applicants.Dtos
{
    public class ApplicantCsvDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Skype { get; set; }
        public double Experience { get; set; }
    }

    public class ApplicantCsvDtoValidator : AbstractValidator<ApplicantCsvDto>
    {
        public ApplicantCsvDtoValidator()
        {
            RuleFor(h => h.FirstName).NotNull().NotEmpty();
            RuleFor(h => h.LastName).NotNull().NotEmpty();
            RuleFor(h => h.MiddleName).NotNull().NotEmpty();
            RuleFor(h => h.BirthDate).NotNull().NotEmpty();
            RuleFor(h => h.Email).NotNull().EmailAddress();
            RuleFor(a => a.Phone).NotNull().NotEmpty();
            RuleFor(a => a.Skype).NotNull().NotEmpty();
            RuleFor(a => a.Experience).GreaterThanOrEqualTo(0);
        }
    }

    public class ApplicantCsvDtoClassMap : ClassMap<ApplicantCsvDto>
    {
        public ApplicantCsvDtoClassMap()
        {
            Map(a => a.FirstName).Name("FirstName");
            Map(a => a.LastName).Name("LastName");
            Map(a => a.MiddleName).Name("MiddleName");
            Map(a => a.BirthDate).Name("BirthDate");
            Map(a => a.Email).Name("Email");
            Map(a => a.Phone).Name("Phone");
            Map(a => a.Skype).Name("Skype");
            Map(a => a.Experience).Name("Experience");
        }
    }
}
