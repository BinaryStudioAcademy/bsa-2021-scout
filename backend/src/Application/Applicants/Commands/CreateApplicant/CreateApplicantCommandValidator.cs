using Application.Common.Files;
using Application.Common.Validators;
using FluentValidation;

namespace Application.Applicants.Commands.Create
{
    public class CreateApplicantCommandValidator : AbstractValidator<CreateApplicantCommand>
    {
        public CreateApplicantCommandValidator()
        {
            RuleFor(_ => _.CvFileDto).ExtensionMustBeInList(new FileExtension[] { FileExtension.Pdf });
            //RuleFor(_ => _.ApplicantDto).SetValidator(new CreateApplicantDtoValidator());
        }
    }
}
