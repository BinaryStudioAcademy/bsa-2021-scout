using Domain.Entities;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.EF;
using Infrastructure.Repositories.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Write
{
    public class ActionWriteRepository : WriteRepository<Action>
    {
        private readonly IConnectionFactory _connectionFactory;
        public ActionWriteRepository(ApplicationDbContext context, IConnectionFactory connectionFactory) : base(context)
        {
            _connectionFactory = connectionFactory;
        }
    }
}
