using Domain.Entities.Mongo;
using Infrastructure.EF;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Write
{
    public class ApplicantCvWriteRepository : MongoWriteRepository<ApplicantCv>
    {
        public ApplicantCvWriteRepository(MongoDbContext context) : base(ApplicantCv.CollectionName, context) { }
    }
}
