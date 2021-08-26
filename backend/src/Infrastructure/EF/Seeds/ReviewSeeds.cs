using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.EF.Seeds
{
    public static class ReviewSeeds
    {
        public static IEnumerable<Review> GetReviews()
        {
            List<Review> list = new List<Review>();

            for (int index = 0; index < 3; index++)
            {
                list.Add(GenerateReview(index));
            }

            return list;
        }

        private static Review GenerateReview(int index)
        {
            return new Review
            {
                Id = reviewIds[index],
                Name = names[index],
            };
        }

        public static List<string> reviewIds = new List<string> {
            "a20246e8-ad19-4583-893b-c1b5b2929416",
            "cef2b5c4-fe62-42ec-99af-a94ac7a4b4e7",
            "98b78eba-d7d9-4a9a-9bc9-81e97ae2c85d",
        };

        public static List<string> names = new List<string> {
            "English",
            "Communication",
            "Tech skills",
        };
    }
}
