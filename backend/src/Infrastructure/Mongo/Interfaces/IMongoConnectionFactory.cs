using MongoDB.Driver;

namespace Infrastructure.Mongo.Interfaces
{
    public interface IMongoConnectionFactory
    {
        IMongoDatabase GetMongoConnection();
    }
}
