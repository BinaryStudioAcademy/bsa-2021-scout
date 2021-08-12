using Application.Common.Files;
using Application.Common.Validators;
using FluentValidation;

namespace Application.Applicants.Commands.UpdateApplicantCv
{
    public class UpdateApplicantCvCommandValidator : AbstractValidator<UpdateApplicantCvCommand>
    {
        public UpdateApplicantCvCommandValidator()
        {
            RuleFor(_ => _.NewCvFileDto).ExtensionMustBeInList(new FileExtension[] { FileExtension.Pdf });
        }
    }
}
