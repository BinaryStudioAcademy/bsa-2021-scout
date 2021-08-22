using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.EF.Seeds
{
    public static class CompanySeeds
    {
        public static IEnumerable<Company> Companies { get; } = new List<Company>
        {
           new Company{
               Id="1",
               Name="Binary Studio",
               Description="Bulka cat is in Lviv",
               Logo = "https://academy.binary-studio.com/static/logo-social.og-aff399bc2ff28efd30a516155a46717a.png"
           }
        };
    }
}