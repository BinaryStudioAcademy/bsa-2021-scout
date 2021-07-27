using System;
using MongoDB.Bson;

namespace Domain.Common
{
    public abstract class MongoEntity
    {
        public ObjectId Id { get; set; }
        public static string CollectionName { get; protected set; }
    }
}
