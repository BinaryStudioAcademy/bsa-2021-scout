using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using Domain.Interfaces;
using Domain.Common;
using Infrastructure.Mongo.Interfaces;

namespace Infrastructure.Repositories.Abstractions
{
    public class MongoReadRespoitory<T> : IReadRepository<T>
        where T : Entity
    {
        private readonly IMongoConnectionFactory _context;

        public MongoReadRespoitory(IMongoConnectionFactory context)
        {
            _context = context;
        }

        public async Task<T> GetAsync(string id)
        {
            ObjectId oid = ObjectId.Parse(id);
            BsonDocument filter = new BsonDocument(new BsonElement("_id", new BsonObjectId(oid)));
            IAsyncCursor<T> cursor = await _context.GetMongoConnection().GetCollection<T>(typeof(T).Name).FindAsync<T>(filter);

            return await cursor.FirstAsync();
        }

        public async Task<IEnumerable<T>> GetEnumerableAsync()
        {
            IAsyncCursor<T> cursor = await _context.GetMongoConnection().GetCollection<T>(typeof(T).Name).FindAsync<T>(new BsonDocument());

            return await cursor.ToListAsync();
        }

        public async Task<T> GetByPropertyAsync(string property, string propertyValue)
        {
            ObjectId oid = ObjectId.Parse(propertyValue);
            BsonDocument filter = new BsonDocument(new BsonElement(string.Format("_{0}", propertyValue), new BsonObjectId(oid)));
            IAsyncCursor<T> cursor = await _context.GetMongoConnection().GetCollection<T>(typeof(T).Name).FindAsync<T>(filter);

            return await cursor.FirstAsync();
        }
    }
}
