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
        protected readonly IMongoConnectionFactory _connectionFactory;

        public MongoWriteRepository(IMongoConnectionFactory context)
        {
            _connectionFactory = context;
        }

        public async Task<T> CreateAsync(T entity)
        {
            if (entity.Id == null || entity.Id == "")
            {
                entity.Id = ObjectId.GenerateNewId().ToString();
            }

            await _connectionFactory
                .GetMongoConnection()
                .GetCollection<T>(typeof(T).Name)
                .InsertOneAsync(entity);

            return entity;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            BsonDocument filter = new BsonDocument(new BsonElement("_id", entity.Id));

            await _connectionFactory
                .GetMongoConnection()
                .GetCollection<T>(typeof(T).Name)
                .ReplaceOneAsync(filter, entity);

            return entity;
        }

        public async Task DeleteAsync(string id)
        {
            ObjectId oid = ObjectId.Parse(id);
            BsonDocument filter = new BsonDocument(new BsonElement("_id", id));

            await _connectionFactory
                .GetMongoConnection()
                .GetCollection<T>(typeof(T).Name)
                .DeleteOneAsync(filter);
        }
    }
}
