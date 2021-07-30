using System.Threading.Tasks;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface ISmtp
    {
        Task SendAsync(string to, string subject, string body, string templateSlug = "default");
        Task SendAsync(IEnumerable<string> to, string subject, string body, string templateSlug = "default");
    }
}
