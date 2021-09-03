using System.Threading.Tasks;
using Domain.Interfaces.Abstractions;
using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Interfaces.Read
{
    public interface IMailTemplateReadRepository : IReadRepository<MailTemplate>
    {
        Task<IEnumerable<MailTemplate>> GetMailTemplatesForThisUser(string userId);
    }
}
