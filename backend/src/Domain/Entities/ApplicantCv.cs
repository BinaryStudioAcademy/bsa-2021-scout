using Domain.Common;

namespace Domain.Entities
{
    public class ApplicantCv : Entity
    {
        public string ApplicantId { get; set; }
        public string Cv { get; set; }
    }
}
