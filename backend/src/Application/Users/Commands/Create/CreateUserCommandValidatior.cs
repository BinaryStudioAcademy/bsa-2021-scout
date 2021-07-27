using Application.Users.Dtos;
using FluentValidation;

namespace Application.Users.Commands.Create
{
    public class CreateUserCommandValidatior : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidatior()
        {
            RuleFor(_ => _.Entity).NotNull();
            RuleFor(_ => _.Entity).SetValidator(new UserDtoValidator());
        }
    }
}
