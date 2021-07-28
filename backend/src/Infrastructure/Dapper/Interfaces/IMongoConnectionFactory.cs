using MongoDB.Driver;

namespace Infrastructure.Dapper.Interfaces
{
    public interface IMongoConnectionFactory
    {
        IMongoDatabase GetMongoConnection();
    }
}
