using Application.Common.Models;

namespace Application.ApplicantCv.Dtos
{
    public class ApplicantCvDto : Dto
    {
        public string ApplicantId { get; set; }
        public string Cv { get; set; }
    }
}
