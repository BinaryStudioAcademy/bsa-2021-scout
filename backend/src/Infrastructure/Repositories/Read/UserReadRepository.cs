using Domain.Entities;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Read
{
    public class UserReadRepository : ReadRepository<User>
    {
        public UserReadRepository(IConnectionFactory connectionFactory) : base("Users", connectionFactory) { }
    }
}
