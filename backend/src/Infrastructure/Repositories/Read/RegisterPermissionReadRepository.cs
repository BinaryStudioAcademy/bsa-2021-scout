using Domain.Entities;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;

namespace Infrastructure.Repositories.Read
{
    public class RegisterPermissionReadRepository : ReadRepository<RegisterPermission>
    {
        public RegisterPermissionReadRepository(IConnectionFactory connectionFactory) : base("RegisterPermissions", connectionFactory) { }

    }
}
