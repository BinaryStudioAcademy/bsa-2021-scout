using Domain.Common;

namespace Domain.Entities.Mongo
{
    public class ApplicantCv : MongoEntity
    {
        public string ApplicantId { get; set; }
        public string Cv { get; set; }

        static ApplicantCv()
        {
            CollectionName = "ApplicantCvs";
        }
    }
}
