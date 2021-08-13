using Dapper;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class EmailTokenReadRepository : ReadRepository<EmailToken>, IReadRepository<EmailToken>
    {
        public EmailTokenReadRepository(IConnectionFactory connectionFactory) : base("EmailToken", connectionFactory) { }

    }
}
