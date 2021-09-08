using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultSharp;

namespace Infrastructure.Vault.Interfaces
{
    public interface IVaultConnectionFactory
    {
        IVaultClient Client { get; }
    }
}
