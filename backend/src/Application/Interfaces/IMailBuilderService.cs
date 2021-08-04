using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMailBuilderService
    {
        Task<string> Build(string body, string templateSlug = "default");
    }
}
