using Infrastructure.Vault.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.Vault.Models.VaultCredentialsWrapper;

namespace Infrastructure.Vault.Context
{
    public class VaultContext : DbContext
    {
        public DbSet<Secret> Secrets { get; set; }

        public VaultContext(string connectionString)
        {
            Database.SetConnectionString(connectionString);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer($"Server=ats_database;Database=HashiCorp;");
            }
        }
    }
}
