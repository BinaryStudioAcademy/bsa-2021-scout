using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Mail;

namespace Application.Interfaces
{
    public interface ISmtp : IDisposable
    {
        Task<string> SendAsync(string to, string subject, string body, string templateSlug = "default", ICollection<Attachment> attachments = null);
        Task<string> SendAsync(IEnumerable<string> to, string subject, string body, string templateSlug = "default", ICollection<Attachment> attachments = null);
    }
}
