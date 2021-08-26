using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Dapper;
using Domain.Interfaces.Read;
using Domain.Entities;
using Application.Common.Exceptions;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using Infrastructure.Mongo.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories.Read
{
    public class MailAttachmentReadRepository : MongoReadRespoitory<MailAttachment>, IMailAttachmentReadRepository
    {
        public MailAttachmentReadRepository(IMongoConnectionFactory connectionFactory) : base(connectionFactory) { }

        public async Task<ICollection<MailAttachment>> GetByMailTemplateIdAsync(string mailTemplateId)
        {
            BsonDocument filter = new BsonDocument(new BsonElement("MailTemplateId", mailTemplateId));

            IAsyncCursor<MailAttachment> cursor = await _connectionFactory
                .GetMongoConnection()
                .GetCollection<MailAttachment>(typeof(MailAttachment).Name)
                .FindAsync<MailAttachment>(new BsonDocument());

            return await cursor.ToListAsync();
        }
    }
}
