namespace Application.Common.Exceptions.Applicants
{
    public class ApplicantPhotoNotFoundException : NotFoundException
    {
        public ApplicantPhotoNotFoundException(string applicantId) : base($"Photo for applicant with id {applicantId} is not found")
        {
        }
    }
}
