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
using Domain.Enums;

namespace Infrastructure.Repositories.Read
{
    public class MailTemplateReadRepository : MongoReadRespoitory<MailTemplate>, IMailTemplateReadRepository
    {
        public MailTemplateReadRepository(IMongoConnectionFactory connectionFactory) : base(connectionFactory) { }

        public async Task<IEnumerable<MailTemplate>> GetMailTemplatesForThisUser(string userId)
        {
            var builder = Builders<MailTemplate>.Filter;
            var filter = builder.Where(x => x.Id != "6130e8ed3c08bd065627b24e"
            && (x.UserCreatedId == userId || x.VisibilitySetting == VisibilitySetting.VisibleForEveryone));

            IAsyncCursor<MailTemplate> cursor = await _connectionFactory
                .GetMongoConnection()
                .GetCollection<MailTemplate>(typeof(MailTemplate).Name)
                .FindAsync<MailTemplate>(filter);

            return await cursor.ToListAsync();
        }
    }
}
