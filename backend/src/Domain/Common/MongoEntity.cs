using MongoDB.Bson;

namespace Domain.Common
{
    public abstract class MongoEntity
    {
        public ObjectId Id { get; set; }
    }
}
