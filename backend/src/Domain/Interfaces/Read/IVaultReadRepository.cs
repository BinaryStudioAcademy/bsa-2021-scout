using System.Threading.Tasks;

namespace Domain.Interfaces.Read
{
    public interface IVaultReadRepository
    {
        Task<string> ReadSecretAsync(string key);
    }
}
