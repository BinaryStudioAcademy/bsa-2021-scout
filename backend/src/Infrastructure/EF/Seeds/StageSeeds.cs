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
           foreach(var id in (new VacancySeeds()).VacancyIds){
               stages.Add( 
                new Stage{
                    Id = id,
                    Name = "Interview",
                    VacancyId = id
                }
            );
           }
        return stages;
        }
    }
}