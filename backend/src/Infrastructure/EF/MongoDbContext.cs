using System;
using MongoDB.Driver;
using Domain.Common;
using Domain.Entities.Mongo;

namespace Infrastructure.EF
{
    public class MongoDbContext
    {
        private readonly string _connectionUri = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_URI");
        private readonly string _database = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME");
        private readonly MongoClient _client;
        private readonly IMongoDatabase _connection;

        public IMongoCollection<ApplicantCv> ApplicantCvs { get; set; }

        public MongoDbContext()
        {
            _client = new MongoClient(_connectionUri);
            _connection = _client.GetDatabase(_database);
            ApplicantCvs = _connection.GetCollection<ApplicantCv>(ApplicantCv.CollectionName);
        }

        public IMongoCollection<T> Collection<T>(string name)
            where T : MongoEntity
        {
            return _connection.GetCollection<T>(name);
        }
    }
}
