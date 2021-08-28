using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.EF.Seeds
{
    public static class VacancyCandidateSeeds
    {
        private static Random _random = new Random();
        private static IList<string> CommentsQuates = new List<string>
        {
            "Coffee addict, no interviews without 3 cups of coffee.",
            "Night owl, preferable interview in afternoon.",
            "Lark, preferable interview early in the morning.",
            "Pet owner, check company pet policy.",
            "Tea addict, 3 cups of tea is must have or else.",
            "Impresive personal achivements.",
            "Quite a procrastinator.",
            "Really punctual",
            "Always late"
        };
        public static IEnumerable<VacancyCandidate> VacancyCandidates { get; } = new List<VacancyCandidate>
        {
          new VacancyCandidate{
              Id = "a0b1e287-893f-52ae-9ab1-a5d9d9f2efc2",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "732f84f8-0625-5098-adbf-4aabcabb5b0c",
              Experience = 2.4,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "ff2eaf94-50fc-5dbe-9175-eb5d1eb36ae3",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "bffcc629-77ef-5304-9d4a-d9b44b5739d6",
              Experience = 1.9,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "7123aeab-e8ab-52be-9911-60128e4c80b1",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "4e5d00dc-f1ab-5a74-9e6f-4edae70fca02",
              Experience = 3.4,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "8b11b446-ebf3-5ca3-8ece-4500063cda5c",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "3e84df9f-f6c3-50d8-8787-d0e2a94af2b6",
              Experience = 2.3,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "548b44af-0e52-5876-ac50-dad780a42387",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "8bf07b5b-af46-58be-b536-58eeca69f661",
              Experience = 1.3,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "51831be3-e5ec-50ce-92fd-87cde7e9a406",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "8be38dd0-8767-54c5-8f78-aa33e11997e9",
              Experience = 5,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "72bf5de1-69c3-5aaa-94a7-bf7b84c185c0",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "c8698455-0a05-503f-a6a6-b8557093a67e",
              Experience = 1.2,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "d3a98231-109b-5db7-9be8-3d4c0b64e70b",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "32e88c57-7a44-51fa-abeb-b9253dacc14f",
              Experience = 1.3,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "dde9ed76-46c9-5b10-8f24-c4f59bd8d68b",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "6a9e4cf9-1a02-50dd-9d32-d82fa566c5a2",
              Experience = 1.5,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "1c9e33cb-a9d0-516c-9f12-45e1c56f9f69",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "5f08c0b7-e605-5672-acce-328b8d660989",
              Experience = 2.4,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "cb47725a-4f3d-5567-b02b-9161330aa821",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "342f8205-6c3f-5b33-8148-bf6fa9ebda64",
              Experience = 1.6,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "19ff5d99-e6ed-5967-808a-3cad8c01a0af",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "8f906dbd-f7dd-54fc-b59f-17d7b1a153dc",
              Experience = 4.3,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "c836afb1-006f-5b92-a7b6-e015cb0fab05",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "eec0eb4b-1999-5b4a-8dad-5f41c8b3e83c",
              Experience = 3.7,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "824e26e9-d5e1-5dcb-a381-7134f2955919",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "388b41ec-993a-5196-9dee-fe831257dda7",
              Experience = 2.8,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "175a15c4-3263-52fb-b368-f534242e651e",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "d6f8a112-c9bb-5e0e-a960-53a9da2495ab",
              Experience = 4.6,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "6723714c-6629-5aa6-a99d-2b5cc62adb3a",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "eccf7842-db64-5e70-b010-67cb3806f2e4",
              Experience = 5.6,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "aa3abdc8-3ae5-50bd-935e-6d5bc36f31ad",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "aa2d6886-ef45-5d0e-a2fa-4c839f5f6e99",
              Experience = 1.9,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "4e64815e-42fa-5ee3-9b0f-3b6f24170536",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "98954928-1256-557a-b79f-7058641f0b4b",
              Experience = 3.7,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "9481253a-bad7-5f1b-b6e0-956a188c1dbb",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "74c1dac0-d4bb-51d9-a2e2-647952974ad6",
              Experience = 3.6,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },
          new VacancyCandidate{
              Id = "f0031f5b-7111-568c-ac2c-645c26d0f479",
              FirstContactDate = null,
              SecondContactDate = null,
              ThirdContactDate = null,
              SalaryExpectation = _random.Next(1200, 56000),
              ApplicantId = "91f8c039-6798-5692-9bb9-68bc16bc4a82",
              Experience = 2.6,
              ContactedById = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              HrWhoAddedId = UserSeeds.GetUsers().Select(x=>x.Id).ToList()[_random.Next(UserSeeds.GetUsers().Count())],
              Comments = CommentsQuates[_random.Next(CommentsQuates.Count())],
              IsViewed = true
          },

        };
    }
}