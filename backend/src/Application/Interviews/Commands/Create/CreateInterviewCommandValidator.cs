using Application.Interviews.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interviews.Commands.Create
{
    public class CreateInterviewCommandValidator : AbstractValidator<CreateInterviewCommand>
    {
        public CreateInterviewCommandValidator()
        {
            RuleFor(_ => _.Interview).NotNull();
            RuleFor(_ => _.Interview).SetValidator(new InterviewDtoValidator());
        }
    }
}
