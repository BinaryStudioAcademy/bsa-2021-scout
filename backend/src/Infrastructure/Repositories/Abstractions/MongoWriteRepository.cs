using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using Domain.Common;
using Domain.Interfaces.Abstractions;
using Infrastructure.Mongo.Interfaces;

namespace Infrastructure.Repositories.Abstractions
{
    public class MongoWriteRepository<T> : IWriteRepository<T>
        where T : Entity
    {
        protected readonly IMongoConnectionFactory _context;

        public MongoWriteRepository(IMongoConnectionFactory context)
        {
            _context = context;
        }

        public async Task<Entity> CreateAsync(T entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            await _context.GetMongoConnection().GetCollection<T>(typeof(T).Name).InsertOneAsync(entity);

            return entity;
        }

        public async Task<Entity> UpdateAsync(T entity)
        {
            BsonDocument filter = new BsonDocument(new BsonElement("_id", entity.Id));
            await _context.GetMongoConnection().GetCollection<T>(typeof(T).Name).ReplaceOneAsync(filter, entity);

            return entity;
        }

        public async Task DeleteAsync(string id)
        {
            ObjectId oid = ObjectId.Parse(id);
            BsonDocument filter = new BsonDocument(new BsonElement("_id", id));

            await _context.GetMongoConnection().GetCollection<T>(typeof(T).Name).DeleteOneAsync(filter);
        }
    }
}
