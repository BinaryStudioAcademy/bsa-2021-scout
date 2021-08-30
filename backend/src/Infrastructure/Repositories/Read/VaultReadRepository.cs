using Domain.Interfaces.Read;
using Infrastructure.Vault.Context;
using Infrastructure.Vault.Interfaces;
using Infrastructure.Vault.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VaultSharp;
using VaultSharp.V1.Commons;
using VaultSharp.V1.SecretsEngines;

namespace Infrastructure.Repositories.Read
{
    public class VaultReadRepository : IVaultReadRepository
    {
        private readonly VaultOptions _config;
        private readonly VaultCredentialsWrapper _credentials;
        private readonly IVaultClient _client;
        public VaultReadRepository(VaultOptions config, VaultCredentialsWrapper credentials, IVaultConnectionFactory connectionFactory)
        {
            _config = config;
            _credentials = credentials;
            _client = connectionFactory.Client;
        }
        public async Task<string> ReadSecretAsync(string key)
        {
            Secret<UsernamePasswordCredentials> dynamicDatabaseCredentials =
                await _client.V1.Secrets.Database.GetCredentialsAsync(
                  _config.Role,
                  _config.MountPath + _config.SecretType);

            _credentials.Username = dynamicDatabaseCredentials.Data.Username;
            _credentials.Password = dynamicDatabaseCredentials.Data.Password;

            using (var context = new VaultContext(
                "Server=ats_database;" +
                "Database=HashiCorp;" +
                $"User id={_credentials.Username};" +
                $"Password={_credentials.Password};")
                )
            {
                return (await context.Set<Secret>().FirstOrDefaultAsync(s => s.Key == key)).Value;
            }
        }
    }
}
