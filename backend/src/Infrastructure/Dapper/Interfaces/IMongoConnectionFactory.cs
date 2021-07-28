using MongoDB.Driver;
using Domain.Common;

namespace Infrastructure.Dapper.Interfaces
{
    public interface IMongoConnectionFactory
    {
        IMongoCollection<T> Collection<T>()
            where T : Entity;
    }
}
