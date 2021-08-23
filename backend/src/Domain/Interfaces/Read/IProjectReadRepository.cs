using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Abstractions;

namespace Domain.Interfaces.Read
{
    public interface IProjectReadRepository : IReadRepository<Project>
    {
        Task<List<Project>> GetByCompanyIdAsync(string id);
    }
}