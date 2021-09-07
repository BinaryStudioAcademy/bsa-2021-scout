using System.Threading.Tasks;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using Domain.Interfaces.Abstractions;
using Domain.Common;
using Application.Common.Exceptions;
using Infrastructure.Mongo.Interfaces;

namespace Infrastructure.Repositories.Abstractions
{
    public class MongoReadRespoitory<T> : IReadRepository<T>
        where T : Entity
    {
        protected readonly IMongoConnectionFactory _connectionFactory;

        public MongoReadRespoitory(IMongoConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<T> GetAsync(string id)
        {
            BsonDocument filter = new BsonDocument(new BsonElement("_id", id));

            IAsyncCursor<T> cursor = await _connectionFactory
                .GetMongoConnection()
                .GetCollection<T>(typeof(T).Name)
                .FindAsync<T>(filter);

            T result = await cursor.FirstOrDefaultAsync();

            if (result == null)
            {
                throw new NotFoundException(typeof(T), id);
            }

            return result;
        }

        public async Task<IEnumerable<T>> GetEnumerableAsync()
        {
            IAsyncCursor<T> cursor = await _connectionFactory
                .GetMongoConnection()
                .GetCollection<T>(typeof(T).Name)
                .FindAsync<T>(new BsonDocument());

            return await cursor.ToListAsync();
        }

        public async Task<T> GetByPropertyAsync(string property, string propertyValue)
        {
            BsonDocument filter = new BsonDocument(new BsonElement(property, propertyValue));

            IAsyncCursor<T> cursor = await _connectionFactory
                .GetMongoConnection()
                .GetCollection<T>(typeof(T).Name)
                .FindAsync<T>(filter);

            return await cursor.FirstAsync();
        }

        public async Task<IEnumerable<T>> GetEnumerableByPropertyAsync(string property, string propertyValue)
        {
            BsonDocument filter = new BsonDocument(new BsonElement(property, propertyValue));

            IAsyncCursor<T> cursor = await _connectionFactory
                .GetMongoConnection()
                .GetCollection<T>(typeof(T).Name)
                .FindAsync<T>(filter);

            return await cursor.ToListAsync();
        }
    }
}
