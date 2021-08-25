using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Read
{
    public interface IUserFollowedReadRepository
    {
        Task<UserFollowedEntity> GetForCurrentUserByTypeAndEntityId(string entityId, EntityType type);
        Task<IEnumerable<UserFollowedEntity>> GetEnumerableForCurrentUserByType(EntityType type);
    }
}
