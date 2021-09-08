using Application.Common.Files;
using Application.Common.Validators;
using FluentValidation;

namespace Application.Applicants.Commands
{
    public class UpdateApplicantPhotoCommandValidator : AbstractValidator<UpdateApplicantPhotoCommand>
    {
        public UpdateApplicantPhotoCommandValidator()
        {
            RuleFor(_ => _.NewPhotoFileDto)
                .ExtensionMustBeInList(new FileExtension[] { FileExtension.Pdf })
                .When(_ => _.NewPhotoFileDto != null);
        }
    }
}
