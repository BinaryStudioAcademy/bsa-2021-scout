using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Domain.Entities;
using Domain.Interfaces.Read;
using Infrastructure.Mongo.Interfaces;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Read
{
    public class MailTemplateReadRepository : MongoReadRespoitory<MailTemplate>, IMailTemplateReadRepository
    {
        public MailTemplateReadRepository(IMongoConnectionFactory context) : base(context) { }

        public async Task<MailTemplate> GetBySlugAsync(string slug)
        {
            BsonDocument filter = new BsonDocument(new BsonElement("Slug", slug));

            IAsyncCursor<MailTemplate> cursor = await _connectionFactory
                .GetMongoConnection()
                .GetCollection<MailTemplate>(typeof(MailTemplate).Name)
                .FindAsync<MailTemplate>(filter);

            return await cursor.FirstAsync();
        }
    }
}
