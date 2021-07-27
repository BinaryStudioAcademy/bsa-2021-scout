using Domain.Entities.Mongo;
using Infrastructure.EF;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Read
{
    public class ApplicantCvReadRepository : MongoReadRespoitory<ApplicantCv>
    {
        public ApplicantCvReadRepository(MongoDbContext context) : base(ApplicantCv.CollectionName, context) { }
    }
}
