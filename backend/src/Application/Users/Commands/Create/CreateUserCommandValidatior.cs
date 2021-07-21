using Application.Users.Dtos;
using FluentValidation;

namespace Application.Users.Commands.Create
{
    public class CreateUserCommandValidatior : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidatior()
        {
            RuleFor(_ => _.User).NotNull();
            RuleFor(_ => _.User).SetValidator(new UserDtoValidator());
        }
    }
}
