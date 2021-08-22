namespace Application.Common.Exceptions.Applicants
{
    public class ApplicantCvNotFoundException : NotFoundException
    {
        public ApplicantCvNotFoundException(string applicantId) : base($"CV for applicant with id {applicantId} is not found")
        {
        }
    }
}
