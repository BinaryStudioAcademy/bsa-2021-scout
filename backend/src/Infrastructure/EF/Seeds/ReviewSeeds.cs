using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.EF.Seeds
{
    public static class ReviewSeeds
    {
        public static IEnumerable<Review> GetReviews()
        {
            List<Review> list = new List<Review>();

            for (int index = 0; index < 8; index++)
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
            "2dd7f1cf-cfd2-4614-989d-ae70c9596893",
            "cef2b5c4-fe62-42ec-99af-a94ac7a4b4e7",
            "7527128e-9d87-4d55-a5db-db7f31e83e77",
            "3579a0fc-2451-4c2c-be2f-456faa6da97f",
            "c1031e15-8aa3-485d-9715-bc2ef06a1468",
            "c3d353e1-21f4-49a0-a74c-c922a7e4b3a2",
            "b714d705-817d-4818-9de3-e595e1b6c48b",
            "15b7992b-af69-43bc-bb3a-bee2640df126",
        };

        public static List<string> names = new List<string> {
            "English",
            "French",
            "Communication skills",
            "Tech skills",
            "Testing skills",
            "Development skills",
            "Backend development skills",
            "Frontend development skills",
        };
    }
}
