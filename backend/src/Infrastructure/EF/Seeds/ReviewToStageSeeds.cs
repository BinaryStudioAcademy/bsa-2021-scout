using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.EF.Seeds
{
    public static class ReviewToStageSeeds
    {
        public static IEnumerable<ReviewToStage> GetReviewToStages()
        {
            List<ReviewToStage> list = new List<ReviewToStage>();

            foreach (string vacancyId in VacancySeeds.vacancyIds)
            {
                string id = vacancyId.Substring(0, vacancyId.Length - 3) + "003";

                list.Add(new ReviewToStage
                {
                    StageId = id,
                    ReviewId = ReviewSeeds.reviewIds[0],
                });

                list.Add(new ReviewToStage
                {
                    StageId = id,
                    ReviewId = ReviewSeeds.reviewIds[2],
                });
            }

            return list;
        }
    }
}
