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
               Description="Bulka cat is in Lviv"
           }
        };
    }
}