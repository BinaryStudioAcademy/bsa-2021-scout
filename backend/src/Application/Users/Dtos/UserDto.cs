using Application.Common.Models;
using FluentValidation;
using System;

namespace Application.Users.Dtos
{
    public class UserDto: Dto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birth { get; set; }
    }

    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(_ => _.FirstName).NotNull().NotEmpty();
            RuleFor(_ => _.LastName).NotNull().NotEmpty();
        }
    }
}
