using Application.Interfaces;
using Dapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Abstractions;
using Domain.Interfaces.Read;
using Infrastructure.Dapper.Interfaces;
using Infrastructure.Repositories.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Read
{
    public class UserFollowedEntityReadRepository : ReadRepository<UserFollowedEntity>, IUserFollowedReadRepository
    {
       protected readonly ICurrentUserContext _currentUserContext;
        public UserFollowedEntityReadRepository(IConnectionFactory connectionFactory, ICurrentUserContext currentUserContext) : base("Vacancies", connectionFactory) {
            _currentUserContext = currentUserContext;
        }

        public async Task<IEnumerable<UserFollowedEntity>> GetEnumerableForCurrentUserByType(EntityType type)
        {
            string hrId = (await _currentUserContext.GetCurrentUser()).Id;
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            StringBuilder followedSql = new StringBuilder();
            followedSql.Append("SELECT UE.* FROM UserFollowedEntities as UE");
            followedSql.Append(" JOIN Users ON Users.Id = UE.UserId");
            followedSql.Append(" WHERE UE.UserId = @UserId AND UE.EntityType = @EntityType");
            IEnumerable<UserFollowedEntity> userFollowedEntityList = (await connection
            .QueryAsync<UserFollowedEntity>(followedSql.ToString(), 
                param: new 
                {
                    UserId = hrId,
                    EntityType = type,
                }
            ));
            await connection.CloseAsync();
            return userFollowedEntityList;
        }

        public async Task<UserFollowedEntity> GetForCurrentUserByTypeAndEntityId(string entityId, EntityType type)
        {
           string hrId = (await _currentUserContext.GetCurrentUser()).Id;
            var connection = _connectionFactory.GetSqlConnection();
            await connection.OpenAsync();
            StringBuilder followedSql = new StringBuilder();
            followedSql.Append("SELECT UE.* FROM UserFollowedEntities as UE");
            followedSql.Append(" JOIN Users ON Users.Id = UE.UserId");
            followedSql.Append(" WHERE UE.UserId = @UserId AND UE.EntityId = @EntityId AND UE.EntityType = @EntityType");
            UserFollowedEntity userFollowedEntity = (await connection
            .QueryAsync<UserFollowedEntity>(followedSql.ToString(), 
                param: new 
                {
                    UserId = hrId,
                    EntityType = type,
                    EntityId = entityId,
                }
            )).FirstOrDefault();
            await connection.CloseAsync();
            return userFollowedEntity;
           
        }
    }
}
