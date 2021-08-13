using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.EF.Seeds
{
    public static class StageSeeds
    {

        public static IEnumerable<Stage> Stages ()
        { 
           var stages = new List<Stage>();
           foreach(var vacancyId in (new VacancySeeds()).VacancyIds){
               stages.Add( 
                new Stage{
                    Id = Guid.NewGuid().ToString(),
                    Name = "Interview",
                    VacancyId = vacancyId
                }
            );
           }
        return stages;
        }
    }
}