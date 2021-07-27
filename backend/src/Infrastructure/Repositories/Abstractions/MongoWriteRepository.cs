using System.Threading.Tasks;
using MongoDB.Bson;
using Domain.Common;
using Domain.Interfaces;
using Infrastructure.EF;

namespace Infrastructure.Repositories.Abstractions
{
    public class MongoWriteRepository<T> : IMongoWriteRepository<T>
        where T : MongoEntity
    {
        private readonly MongoDbContext _context;
        private readonly string _collectionName;

        public MongoWriteRepository(string collectionName, MongoDbContext context)
        {
            _collectionName = collectionName;
            _context = context;
        }

        public async Task<MongoEntity> CreateAsync(T entity)
        {
            await _context.Collection<T>(_collectionName).InsertOneAsync(entity);

            return entity;
        }

        public async Task<MongoEntity> UpdateAsync(T entity)
        {
            BsonDocument filter = new BsonDocument(new BsonElement("_id", new BsonObjectId(entity.Id)));
            await _context.Collection<T>(_collectionName).ReplaceOneAsync(filter, entity);

            return entity;
        }

        public async Task DeleteAsync(ObjectId id)
        {
            BsonDocument filter = new BsonDocument(new BsonElement("_id", new BsonObjectId(id)));

            await _context.Collection<T>(_collectionName).DeleteOneAsync(filter);
        }
    }
}
