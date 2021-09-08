using System.Collections.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.EF.Seeds
{
    public static class StageSeeds
    {
        public static IEnumerable<Stage> GetStages()
        {
            IList<Stage> stages = new List<Stage>();
            foreach (string id in VacancySeeds.vacancyIds)
            {
                for (int index = 0; index < types.Count; index++)
                {
                    stages.Add(
                        new Stage
                        {
                            Id = id.Substring(0, id.Length - 3) + "00" + index.ToString(),
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
            "Self-Applied",
            "Applied",
            "Phone screen",
            "Interview",
            "Offer",
            "Hired",
        };

        public static List<StageType> types = new List<StageType> {
            StageType.Applied,
            StageType.PhoneScreen,
            StageType.Interview,
            StageType.Offer,
            StageType.Hired,
        };
    }
}