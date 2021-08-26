using System.Threading.Tasks;
using Domain.Interfaces.Abstractions;
using Domain.Entities;
using System.Collections.Generic;

namespace Domain.Interfaces.Read
{
    public interface IMailAttachmentReadRepository : IReadRepository<MailAttachment>
    {
        Task<ICollection<MailAttachment>> GetByMailTemplateIdAsync(string mailTemplateId);
    }
}
