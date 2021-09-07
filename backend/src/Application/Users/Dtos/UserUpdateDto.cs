using Application.Common.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.Dtos
{
    public class UserUpdateDto : Dto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Phone { get; set; }
        public string Skype { get; set; }
        public string Slack { get; set; }
        public bool? IsImageToDelete { get; set; }


    }

    public class UserUpdateDtoValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateDtoValidator()
        {
            RuleFor(a => a.FirstName).NotNull().NotEmpty();
            RuleFor(a => a.LastName).NotNull().NotEmpty();

        }
    }
}
