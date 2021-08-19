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
          IList<Stage> stages = new List<Stage>();
          foreach (string id in (new VacancySeeds()).VacancyIds)
            {
                for (int index = 0; index < Types.Count; index++)
                {
                    stages.Add(
                        new Stage
                        {
                            Id = id.Substring(0, id.Length - 1) + index.ToString(),
                            Name = Names[index],
                            Type = Types[index],
                            Index = index,
                            IsReviewable = index == 3,
                            VacancyId = id,
                        }
                    );
                }
            }

            return stages;
        }
        private static List<string> Names = new List<string> {
            "Applied",
            "Phone screen",
            "Interview",
            "Test",
            "Offer",
            "Hired",
        };
        public static List<StageType> Types = new List<StageType> {
            StageType.Applied,
            StageType.PhoneScreen,
            StageType.Interview,
            StageType.Test,
            StageType.Offer,
            StageType.Hired,
        };
    }
}