using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.Elastic.Seeding
{
    public static class ApplicantTagsSeeds
    {
        #region Tags
        private readonly static IList<Tag> Tags = new List<Tag>
        {
            new Tag
            {
                Id = "f926cc1a-d5e5-4753-b1ff-6af2baacfe13",
                TagName = "Web Developer",
            },
            new Tag
            {
                Id = "c53fd98d-6324-43f6-a310-e8099e428e31",
                TagName = "Fullstack"
            },
            new Tag
            {
                Id = "f926cc1a-d5e5-4753-b1ff-6af2baacfe13",
                TagName = "Angular/Dotnet"
            },
            new Tag
            {
                Id = "b62e6e4f-9dc2-4418-80d0-b787279a676e",
                TagName = "UI/UX"
            },
            new Tag
            {
                Id = "528d29f8-c790-4a09-b115-1a992c03a225",
                TagName = "Devops"
            },
            new Tag
            {
                Id = "4e20103e-09a4-4fb2-adcc-4b3793725c1d",
                TagName = "QA"
            },
            new Tag
            {
                Id = "80714c7c-8b25-470d-983e-2f43576483c8",
                TagName = "Product manager"
            }
        };
        #endregion

        public static IEnumerable<ElasticEntity> GetSeed()
        {
            return new List<ElasticEntity>
            {
                new ElasticEntity
                {
                    Id = "f0efedc7-a5c4-4e8c-a1d7-d071c9a474a1",
                    Tags = new List<Tag>
                    {
                        Tags[0],
                        Tags[2]
                    },
                    ElasticType = ElasticType.ApplicantTags
                },
                new ElasticEntity
                {
                    Id = "16a4547b-cde7-40c2-8249-ec8e331c22e1",
                    Tags = new List<Tag>
                    {
                        Tags[1],
                        Tags[3]
                    },
                    ElasticType = ElasticType.VacancyTags
                },
                new ElasticEntity
                {
                    Id = "0de2cfc2-0121-4e0d-8417-2bd5d466cbd1",
                    Tags = new List<Tag>
                    {
                        Tags[4]
                    },
                    ElasticType = ElasticType.VacancyTags
                },
                new ElasticEntity
                {
                    Id = "ed6ab1ee-a95a-45b4-ab8c-c0c23fcc41f1",
                    Tags = new List<Tag>
                    {
                        Tags[2],
                        Tags[3],
                        Tags[5]
                    },
                    ElasticType = ElasticType.ApplicantTags
                },
                new ElasticEntity
                {
                    Id = "8e182e40-78d2-4ff5-a3b4-670dfe5cad31",
                    Tags = new List<Tag>
                    {
                        Tags[3],
                        Tags[2],
                        Tags[4]
                    },
                    ElasticType = ElasticType.ApplicantTags
                },
                new ElasticEntity
                {
                    Id = "6cfe6a66-714f-46a5-aa85-8755a3a110f1",
                    Tags = new List<Tag>
                    {
                        Tags[1],
                        Tags[4]
                    },
                    ElasticType = ElasticType.VacancyTags
                },
                new ElasticEntity{
                    Id = "15edec87-538a-41af-971f-919712ad0fb1",
                    Tags = new List<Tag>{
                        Tags[0]
                    },
                    ElasticType = ElasticType.ApplicantTags
                }
            };
        }
    }
}