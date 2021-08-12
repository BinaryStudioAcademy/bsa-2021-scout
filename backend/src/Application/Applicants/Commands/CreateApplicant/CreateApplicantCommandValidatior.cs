using Application.Applicants.Dtos;
using Application.Common.Commands;
using Application.Common.Validators;
using Application.Users.Dtos;
using FluentValidation;

namespace Application.Applicants.Commands.Create
{
    public class CreateApplicantCommandValidatior : AbstractValidator<CreateApplicantCommand>
    {
        public CreateApplicantCommandValidatior()
        {
            RuleFor(_ => _.CvFileDto).ExtensionMustBeInList(new string[] { ".pdf" });
            //RuleFor(_ => _.ApplicantDto).SetValidator(new CreateApplicantDtoValidator());
        }
    }
}
