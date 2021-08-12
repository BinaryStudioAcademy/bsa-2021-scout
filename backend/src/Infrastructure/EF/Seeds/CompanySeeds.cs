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
               Id="5fc2b47f-4e39-4d14-9026-15f9a259a9d9",
               Name="Binary Studio",
               Description="Bulka cat is in Lviv"
           }
        };
    }
}