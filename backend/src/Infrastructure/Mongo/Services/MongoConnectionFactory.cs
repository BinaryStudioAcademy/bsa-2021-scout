using System;
using MongoDB.Driver;
using Infrastructure.Mongo.Interfaces;

namespace Infrastructure.Mongo.Services
{
    public class MongoConnectionFactory : IMongoConnectionFactory
    {
        private readonly string _connectionUri;
        private readonly string _databaseName;
        private IMongoDatabase _database;

        public MongoConnectionFactory()
        {
            _connectionUri = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_URI");
            _databaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME");

            if (_connectionUri is null)
                throw new Exception("Database connection string is not specified");

            if (_databaseName is null)
                throw new Exception("Mongo database name is not specified");
        }

        public IMongoDatabase GetMongoConnection()
        {
            return _database ??= new MongoClient(_connectionUri).GetDatabase(_databaseName);
        }
    }
}
