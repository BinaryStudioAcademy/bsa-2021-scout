using Domain.Entities.Abstractions;

namespace Domain.Entities
{
    public class CvParsingJob : Job
    {
        public string AWSJobId { get; set; }
        public string FilePath { get; set; }
    }
}
