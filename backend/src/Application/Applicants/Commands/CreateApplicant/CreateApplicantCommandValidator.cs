using Application.Common.Files;
using Application.Common.Validators;
using FluentValidation;

namespace Application.Applicants.Commands.CreateApplicant
{
    public class CreateApplicantCommandValidator : AbstractValidator<CreateApplicantCommand>
    {
        public CreateApplicantCommandValidator()
        {
            RuleFor(_ => _.CvFileDto)
                .ExtensionMustBeInList(new FileExtension[] { FileExtension.Pdf })
                .When(_ => _.CvFileDto != null);

            RuleFor(_ => _.PhotoFileDto)
                .ExtensionMustBeInList(new FileExtension[] {
                    FileExtension.Png,
                    FileExtension.Jpg,
                    FileExtension.Jpeg
                })
                .When(_ => _.PhotoFileDto != null);
        }
    }
}
