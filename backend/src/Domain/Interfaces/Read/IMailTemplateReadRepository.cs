using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Abstractions;

namespace Domain.Interfaces.Read
{
    public interface IMailTemplateReadRepository : IReadRepository<MailTemplate>
    {
        Task<MailTemplate> GetBySlugAsync(string slug);
    }
}
