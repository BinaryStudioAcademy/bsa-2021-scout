using Infrastructure.Vault.Interfaces;
using Infrastructure.Vault.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Vault.Extensions
{
    public static class VaultConfigurationExtension
    {
        public static IServiceCollection ConfigureVault(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IVaultConnectionFactory, VaultConnectionFactory>();

            var vaultOptions = configuration.GetSection("Vault");

            var options = new VaultOptions()
            {
                Role = vaultOptions["Role"],
                MountPath = vaultOptions["MountPath"],
                SecretType = vaultOptions["SecretType"]
            };
            services.AddSingleton(options);

            var credentials = new VaultCredentialsWrapper();
            services.AddSingleton(credentials);

            return services;
        }
    }
}
