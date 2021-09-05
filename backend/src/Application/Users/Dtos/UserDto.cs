using Application.Common.Models;
using FluentValidation;
using System;
using System.Collections.Generic;

namespace Application.Users.Dtos
{
    public class UserDto : Dto
    {
        public UserDto()
        {
            Roles = new List<RoleDto>();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string Email { get; set; }
        public string CompanyId { get; set; }
        public string Phone { get; set; }
        public string Skype { get; set; }
        public string Slack { get; set; }
        public string AvatarUrl { get; set; }

        public ICollection<RoleDto> Roles { get; set; }

        public bool IsEmailConfirmed {get ; set;}

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
