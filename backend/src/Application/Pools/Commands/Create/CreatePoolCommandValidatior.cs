using Application.Common.Commands;
using Application.Pools.Dtos;
using FluentValidation;

namespace Application.Users.Commands.Create
{
    public class CreaterPoolCommandValidatior : AbstractValidator<CreateEntityCommand<CreatePoolDto>>
    {
        public CreaterPoolCommandValidatior()
        {
            RuleFor(_ => _.Entity).NotNull();
            RuleFor(_ => _.Entity).SetValidator(new CreatePoolDtoValidator());
        }
    }
}
