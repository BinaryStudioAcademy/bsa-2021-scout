using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class ActionReadRepository : ReadRepository<Action>, IReadRepository<Action>
    {
        public ActionReadRepository(IConnectionFactory connectionFactory) : base("Actions", connectionFactory) { }
    }
}
