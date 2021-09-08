using Application.Common.Commands;
using Application.Tasks.Dtos;
using FluentValidation;

namespace Application.Users.Commands.Create
{
    public class CreaterTaskCommandValidatior : AbstractValidator<CreateEntityCommand<CreateTaskDto>>
    {
        public CreaterTaskCommandValidatior()
        {
            RuleFor(_ => _.Entity).NotNull();
            RuleFor(_ => _.Entity).SetValidator(new CreateTaskDtoValidator());
        }
    }
}
