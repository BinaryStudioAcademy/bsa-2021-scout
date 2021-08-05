using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Domain.Entities;
using Domain.Interfaces.Read;
using Infrastructure.Mongo.Interfaces;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Read
{
    public class ApplicantCvReadRepository : MongoReadRespoitory<ApplicantCv>, IApplicantCvReadRepository
    {
        public ApplicantCvReadRepository(IMongoConnectionFactory connectionFactory) : base(connectionFactory) { }

        public async Task<ApplicantCv> GetByApplicantAsync(string applicantId)
        {
            BsonDocument filter = new BsonDocument(new BsonElement("ApplicantId", applicantId));

            IAsyncCursor<ApplicantCv> cursor = await _connectionFactory
                .GetMongoConnection()
                .GetCollection<ApplicantCv>(typeof(ApplicantCv).Name)
                .FindAsync<ApplicantCv>(filter);

            ApplicantCv result = await cursor.FirstOrDefaultAsync();

            return result;
        }
    }
}
