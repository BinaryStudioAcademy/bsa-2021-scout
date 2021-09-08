using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISmtpFactory
    {
        Task<ISmtp> Connect();
        ISmtp Connect(string address, string password, string displayName);
    }
}
