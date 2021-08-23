using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.EF.Seeds
{
    public static class CompanySeeds
    {
        public static IEnumerable<Company> GetCompanies()
        {
            return new List<Company>
            {
                new Company {
                    Id = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Name = "Binary Studio",
                    Description = "Bulka cat is in Lviv",
                    Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
                }
            };
        }
    }
}
