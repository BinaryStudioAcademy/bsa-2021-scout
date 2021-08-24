using Microsoft.AspNetCore.Http;

namespace Application.ApplicantCvs.Dtos
{
    public class ApplicantCvOnlyFileDto
    {
        public IFormFile File { get; set; }
    }
}
