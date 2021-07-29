using Domain.Common;

namespace Domain.Entities
{
    public class PoolToApplicant : Entity
    {
        public string PoolId { get; set; }
        public string ApplicantId { get; set; }

        public Pool Pool { get; set; }
        public Applicant Applicant { get; set; }
    }
}