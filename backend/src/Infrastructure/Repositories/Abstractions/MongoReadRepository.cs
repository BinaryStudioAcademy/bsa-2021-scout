using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using Domain.Interfaces;
using Domain.Common;
using Infrastructure.EF;

namespace Infrastructure.Repositories.Abstractions
{
    public class MongoReadRespoitory<T> : IMongoReadRepository<T>
        where T : MongoEntity
    {
        private readonly MongoDbContext _context;
        private readonly string _collectionName;

        public MongoReadRespoitory(string collectionName, MongoDbContext context)
        {
            _collectionName = collectionName;
            _context = context;
        }

        public async Task<T> GetAsync(ObjectId id)
        {
            BsonDocument filter = new BsonDocument(new BsonElement("_id", new BsonObjectId(id)));
            IAsyncCursor<T> cursor = await _context.Collection<T>(_collectionName).FindAsync<T>(filter);

            return await cursor.FirstAsync();
        }

        public async Task<IEnumerable<T>> GetEnumerableAsync()
        {
            IAsyncCursor<T> cursor = await _context.Collection<T>(_collectionName).FindAsync<T>(new BsonDocument());

            return await cursor.ToListAsync();
        }
    }
}
