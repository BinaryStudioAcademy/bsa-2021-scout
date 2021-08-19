using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.EF.Seeds
{
    public static class PoolSeeds
    {
        public static IEnumerable<Pool> Pools { get; } = new List<Pool>
        {
          new Pool{
              Id = "23f7b492-983d-512a-872e-4b2d42f156ba",
              CompanyId = "1"
          }
        };
    }
}