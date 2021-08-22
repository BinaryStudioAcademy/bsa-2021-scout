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
              Name = "Extra valuable",
              DateCreated = new DateTime(2021, 4, 13),
              CreatedById = "057c23ff-58b1-4531-8012-0b5c1f949ee1",
              Description = "This pool contains extra valuable candidates for current company.",
              CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4"
          },
          new Pool{
              Id = "15c616ce-669b-5d10-9612-8d7ed29666f5",
              Name = "Questionable",
              DateCreated = new DateTime(2021, 5, 14),
              CreatedById = "fb055f22-c5d1-4611-8e49-32a46edf58b2",
              Description = "This candidates fully qualified, but might have Work Visa problems.",
              CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4"
          },
          new Pool{
              Id = "0c32a3b2-80e9-5143-91bc-179e7df275d7",
              Name = "With big time gap",
              DateCreated = new DateTime(2021, 4, 25),
              CreatedById = "ac78dabf-929c-4fa5-9197-7d14e18b40ab",
              Description = "Candidate have enough work experience, however there is a big time gap from previous employment.",
              CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4"
          },
          new Pool{
              Id = "44e8870c-afda-5328-8b82-64be589af4a0",
              Name = "Ordinary candidates",
              DateCreated = new DateTime(2021, 7, 5),
              CreatedById = "f8afcbaa-54bd-4103-b3f0-0dd17a5226ca",
              Description = "Just an average candidate.",
              CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4"
          },
          new Pool{
              Id = "a661cd02-c014-5c13-8fc3-99febd62d469",
              Name = "Reapplied",
              DateCreated = new DateTime(2021, 8, 13),
              CreatedById = "8be08dba-5dac-408a-ab6e-8d162af74024",
              Description = "Candidate was working for the current company, than leaved and now reappling again.",
              CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4"
          },
        };
    }
}