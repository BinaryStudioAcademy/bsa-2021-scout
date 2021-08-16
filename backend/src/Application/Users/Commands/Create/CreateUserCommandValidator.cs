using Application.Common.Commands;
using Application.Users.Dtos;
using FluentValidation;

namespace Application.Users.Commands.Create
{
    public class CreateUserCommandValidator : AbstractValidator<CreateEntityCommand<UserDto>>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(_ => _.Entity).NotNull();
            RuleFor(_ => _.Entity).SetValidator(new UserDtoValidator());
        }
    }
}
