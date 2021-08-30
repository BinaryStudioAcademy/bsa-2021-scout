﻿using Infrastructure.Vault.Interfaces;
using System;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Infrastructure.Vault
{
    public class VaultConnectionFactory : IVaultConnectionFactory
    {
        private readonly IVaultClient _client;
        private readonly VaultClientSettings _settings;

        public VaultClientSettings Settings => _settings ?? 
            new VaultClientSettings(
                Environment.GetEnvironmentVariable("VAULT_ADDR"),
                new TokenAuthMethodInfo("some-root-token")
            );

        public IVaultClient Client => _client ?? new VaultClient(Settings);

        public VaultConnectionFactory()
        {
            _settings = new VaultClientSettings(
                Environment.GetEnvironmentVariable("VAULT_ADDR"),
                new TokenAuthMethodInfo("some-root-token")
            );

            _client = new VaultClient(_settings);
        }
    }
}
