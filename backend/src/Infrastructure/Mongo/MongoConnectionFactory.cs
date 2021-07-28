using System;
using MongoDB.Driver;
using Domain.Common;
using Domain.Entities;

namespace Infrastructure.Mongo
{
    public class MongoConnectionFactory
    {
        private readonly string _connectionUri = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_URI");
        private readonly string _database = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME");
        private readonly MongoClient _client;
        private readonly IMongoDatabase _connection;

        public IMongoCollection<ApplicantCv> ApplicantCvs { get; set; }

        public MongoConnectionFactory()
        {
            _client = new MongoClient(_connectionUri);
            _connection = _client.GetDatabase(_database);
            ApplicantCvs = _connection.GetCollection<ApplicantCv>(nameof(ApplicantCv));
        }

        public IMongoCollection<T> Collection<T>()
            where T : Entity
        {
            return _connection.GetCollection<T>(nameof(T));
        }
    }
}
