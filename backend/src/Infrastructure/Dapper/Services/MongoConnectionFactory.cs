using System;
using MongoDB.Driver;
using Domain.Common;
using Infrastructure.Dapper.Interfaces;

namespace Infrastructure.Dapper.Services
{
    public class MongoConnectionFactory : IMongoConnectionFactory
    {
        private readonly string _connectionUri;
        private readonly string _database;

        public MongoConnectionFactory()
        {
            _connectionUri = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_URI");
            _database = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME");

            if (_connectionUri is null)
                throw new Exception("Database connection string is not specified");

            if (_database is null)
                throw new Exception("Mongo database name is not specified");
        }

        public IMongoCollection<T> Collection<T>()
            where T : Entity
        {
            MongoClient client = new MongoClient(_connectionUri);

            return client.GetDatabase(_database).GetCollection<T>(typeof(T).Name);
        }
    }
}
