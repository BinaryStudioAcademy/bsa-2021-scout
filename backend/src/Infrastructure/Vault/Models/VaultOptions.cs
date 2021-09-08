using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Vault.Models
{
    public class VaultOptions
    {
        public string Role { get; set; }
        public string MountPath { get; set; }
        public string SecretType { get; set; }
    }
}
