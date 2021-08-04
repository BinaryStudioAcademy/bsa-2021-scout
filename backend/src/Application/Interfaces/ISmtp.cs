using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ISmtp : IDisposable
    {
        Task<string> SendAsync(string to, string subject, string body, string templateSlug = "default");
        Task<string> SendAsync(IEnumerable<string> to, string subject, string body, string templateSlug = "default");
    }
}
