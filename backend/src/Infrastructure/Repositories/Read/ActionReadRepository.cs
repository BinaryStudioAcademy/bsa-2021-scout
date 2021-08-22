using Domain.Entities;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class ActionReadRepository : ReadRepository<Action>
    {
        public ActionReadRepository(IConnectionFactory connectionFactory) : base("Actions", connectionFactory) { }
    }
}
