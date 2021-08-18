using System.Collections.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.EF.Seeds
{
    public static class StageSeeds
    {
        public static IEnumerable<Stage> GetStages()
        {
            List<Stage> stages = new List<Stage>();

            foreach (string id in VacancySeeds.vacancyIds)
            {
                for (int index = 0; index < 6; index++)
                {
                    stages.Add(
                        new Stage
                        {
                            Id = id.Substring(0, id.Length - 1) + index.ToString(),
                            Name = names[index],
                            Type = types[index],
                            Index = index,
                            IsReviewable = index == 3,
                            VacancyId = id,
                        }
                    );
                }
            }

            return stages;
        }

        private static List<string> names = new List<string> {
            "Applied",
            "Phone screen",
            "Interview",
            "Test",
            "Offer",
            "Hired",
        };

        private static List<StageType> types = new List<StageType> {
            StageType.Applied,
            StageType.PhoneScreen,
            StageType.Interview,
            StageType.Test,
            StageType.Offer,
            StageType.Hired,
        };
    }
}