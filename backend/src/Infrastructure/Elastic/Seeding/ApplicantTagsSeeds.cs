using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.Elastic.Seeding
{
    public static class ElasticTagsSeeds
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
                Id = "f926cc1a-d5e5-4753-b1ff-6af2baacf213",
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
                    Id = "732f84f8-0625-5098-adbf-4aabcabb5b0c",
                    Tags = new List<Tag>
                    {
                        Tags[0],
                        Tags[2]
                    },
                    ElasticType = ElasticType.ApplicantTags
                },
                new ElasticEntity
                {
                    Id = "bffcc629-77ef-5304-9d4a-d9b44b5739d6",
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
                    Id = "4e5d00dc-f1ab-5a74-9e6f-4edae70fca02",
                    Tags = new List<Tag>
                    {
                        Tags[3],
                        Tags[2],
                        Tags[4]
                    },
                    ElasticType = ElasticType.ApplicantTags
                },

                new ElasticEntity{
                    Id = "3e84df9f-f6c3-50d8-8787-d0e2a94af2b6",
                    Tags = new List<Tag>{
                        Tags[1]
                    },
                    ElasticType = ElasticType.ApplicantTags
                },
                new ElasticEntity
                {
                    Id = "8bf07b5b-af46-58be-b536-58eeca69f661",
                    Tags = new List<Tag>
                    {
                        Tags[3],
                        Tags[2]
                    },
                    ElasticType = ElasticType.ApplicantTags
                },
                new ElasticEntity{
                    Id = "8be38dd0-8767-54c5-8f78-aa33e11997e9",
                    Tags = new List<Tag>{
                        Tags[3],
                        Tags[4]
                    },
                    ElasticType = ElasticType.ApplicantTags
                },
                new ElasticEntity
                {
                    Id = "c17322f5-1d00-42ce-b1d8-b2cad9a72f32",
                    Tags = new List<Tag>
                    {
                        Tags[1],
                        Tags[4]
                    },
                    ElasticType = ElasticType.VacancyTags
                },
                new ElasticEntity
                {
                    Id = "d701f2d8-11ca-49c4-85aa-c4593cace3a9",
                    Tags = new List<Tag>
                    {
                        Tags[1],
                        Tags[3]
                    },
                    ElasticType = ElasticType.VacancyTags
                },
                new ElasticEntity
                {
                    Id = "db1646f4-1c00-4d72-832f-9fd1093ae6dc",
                    Tags = new List<Tag>
                    {
                        Tags[4],
                        Tags[3],
                        Tags[2],
                        Tags[0]
                    },
                    ElasticType = ElasticType.VacancyTags
                },
            };
        }
    }
}