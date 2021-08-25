using System;
using FluentValidation;

namespace Application.Common.Models
{
    public abstract class HumanDto : Dto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        
    }

    public class HumanDtoValidator : AbstractValidator<HumanDto>
    {
        public HumanDtoValidator()
        {
            RuleFor(h => h.FirstName).NotNull().NotEmpty();
            RuleFor(h => h.LastName).NotNull().NotEmpty();
            RuleFor(h => h.BirthDate).NotNull().NotEmpty();
            RuleFor(h => h.Email).NotNull().EmailAddress();
        }
    }
}