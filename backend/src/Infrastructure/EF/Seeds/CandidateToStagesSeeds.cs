using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.EF.Seeds
{
    public static class CandidateToStagesSeeds
    {
        private static Random _random = new Random();
        public static IEnumerable<CandidateToStage> CandidateToStages { get; } = new List<CandidateToStage>
        {
          new CandidateToStage
          {
              Id = "b64463e9-5e91-5bc2-8b94-cc9ed49034cd",
              CandidateId = "a0b1e287-893f-52ae-9ab1-a5d9d9f2efc2",
              StageId = "0c1bde8f-1622-44b3-86d0-67ec69665130",
              DateAdded = new DateTime(2021, 7, 23),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "cb95781e-6896-575e-be24-ae832c4c4096",
              CandidateId = "a0b1e287-893f-52ae-9ab1-a5d9d9f2efc2",
              StageId = "0c1bde8f-1622-44b3-86d0-67ec69665133",
              DateAdded = new DateTime(2021, 4, 24),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "641a94fc-712d-5876-a9e9-c089ecb14946",
              CandidateId = "ff2eaf94-50fc-5dbe-9175-eb5d1eb36ae3",
              StageId = "13f41ed3-108c-4100-b030-197773798d42",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "7b7d481a-0f21-543f-a90b-e6acdfdc27ed",
              CandidateId = "7123aeab-e8ab-52be-9911-60128e4c80b1",
              StageId = "13f41ed3-108c-4100-b030-197773798d44",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "eaa65107-b064-5f08-afd1-0630e43eff7f",
              CandidateId = "8b11b446-ebf3-5ca3-8ece-4500063cda5c",
              StageId = "154065a4-1f7d-4706-b9fd-7a97646870d3",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "25064f25-16e2-5f79-ab93-0f63c6781959",
              CandidateId = "548b44af-0e52-5876-ac50-dad780a42387",
              StageId = "1d4128f9-3d3f-4b19-91f3-f1766e425764",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "e48f5a98-34a0-5b81-bc01-3ee2cba374d7",
              CandidateId = "51831be3-e5ec-50ce-92fd-87cde7e9a406",
              StageId = "25faa343-1b96-4679-9ca9-dc9af08f8912",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "c7d65492-b0bf-532c-8936-8619d2e1b95d",
              CandidateId = "72bf5de1-69c3-5aaa-94a7-bf7b84c185c0",
              StageId = "27730877-1d1e-46ee-a8cb-9b019be065f2",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "92fe0bd6-a17c-503d-a2f2-956562e949bf",
              CandidateId = "d3a98231-109b-5db7-9be8-3d4c0b64e70b",
              StageId = "28c49bd7-17da-415b-88bf-5844843d5fa1",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "2eab9703-e29a-5307-920a-683c3109249d",
              CandidateId = "dde9ed76-46c9-5b10-8f24-c4f59bd8d68b",
              StageId = "32aa24b6-1f28-4be3-9cf9-b09e044f47f0",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "65123c11-5253-5d3e-bd57-1fc48d04f2a5",
              CandidateId = "1c9e33cb-a9d0-516c-9f12-45e1c56f9f69",
              StageId = "34366ad0-15ed-4775-b0cf-826f84b1c092",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "d60c523c-c65b-5725-8d9c-50d67bf3c2b3",
              CandidateId = "cb47725a-4f3d-5567-b02b-9161330aa821",
              StageId = "6532e19d-1bc1-482d-9413-cbf7b5d547c3",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "f5cac2c1-463c-53e0-978f-6172d43b1f43",
              CandidateId = "19ff5d99-e6ed-5967-808a-3cad8c01a0af",
              StageId = "90a4b78a-1bb4-4321-891d-8f36b67f2c34",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "529ca1ef-0c3b-57bf-8106-f52e6fc0d39f",
              CandidateId = "c836afb1-006f-5b92-a7b6-e015cb0fab05",
              StageId = "a43a0590-1aad-4564-94d0-a9e5e6e51c15",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "2f6284de-814d-5255-9f52-7a5c2df137e6",
              CandidateId = "824e26e9-d5e1-5dcb-a381-7134f2955919",
              StageId = "c0444497-1e0f-4b93-83ab-f5522c4391d7",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "13dc21cb-efff-5dcb-9972-cc600ffdd079",
              CandidateId = "175a15c4-3263-52fb-b368-f534242e651e",
              StageId = "c17322f5-1d00-42ce-b1d8-b2cad9a72f31",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "9dcecf91-9562-559b-a203-8f30a676c8e6",
              CandidateId = "6723714c-6629-5aa6-a99d-2b5cc62adb3a",
              StageId = "d12218ee-3688-42d1-8874-fd80a1634342",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "8da744f5-5f3b-50a8-b034-de27552ae2d0",
              CandidateId = "aa3abdc8-3ae5-50bd-935e-6d5bc36f31ad",
              StageId = "d701f2d8-11ca-49c4-85aa-c4593cace3a5",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "a968912a-d065-5872-bc26-cd3678ea80fe",
              CandidateId = "4e64815e-42fa-5ee3-9b0f-3b6f24170536",
              StageId = "db1646f4-1c00-4d72-832f-9fd1093ae6d4",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "22d59065-ed24-55da-a0b6-306edc6d80e8",
              CandidateId = "9481253a-bad7-5f1b-b6e0-956a188c1dbb",
              StageId = "e0a4b78a-1bb4-4321-891d-8f36b67f2c30",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },
          new CandidateToStage
          {
              Id = "aa78adcf-a30c-5eb5-8018-71a84de630db",
              CandidateId = "f0031f5b-7111-568c-ac2c-645c26d0f479",
              StageId = "e2aa24b6-1f28-4be3-9cf9-b09e044f47f1",
              DateAdded = new DateTime(2021, 8, 15),
              DateRemoved = null,
          },

        };
    }
}