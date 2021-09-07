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
        //foreach candidate choose or not random stage by vacancy id
        public static IEnumerable<CandidateToStage> CandidateToStages()
        {
            IList<CandidateToStage> candidateToStages = new List<CandidateToStage>();
            IList<string> vacancyIds = VacancySeeds.vacancyIds;
            IList<Stage> stages = StageSeeds.GetStages().ToList();
            IList<string> userIds = UserSeeds.GetUsers().Select(u => u.Id).ToList();
            Random random = new Random();

            foreach (var candidate in VacancyCandidateSeeds.VacancyCandidates)
            {
                foreach (var vacancyId in vacancyIds)
                {
                    bool isAppliedForVacancy = _random.Next() % 3 == 0;
                    if (!isAppliedForVacancy)
                        continue;
                    string stageId = vacancyId.Substring(0, vacancyId.Length - 3) + "00" + (_random.Next(StageSeeds.types.Count() - 1) + 1);

                    var date = Common.GetRandomDateTime(new DateTime(2021, 04, 03), new DateTime(2021, 06, 29));
                    var dateRemoved = date.AddDays(_random.Next(20));
                    candidateToStages.Add(
                    new CandidateToStage
                    {
                        Id = candidate.Id.Substring(0, candidate.Id.Length - 5) + "stage" + vacancyId.Substring(0, 3),
                        StageId = vacancyId.Substring(0, vacancyId.Length - 3) + "000",
                        CandidateId = candidate.Id,
                        MoverId = userIds[random.Next(userIds.Count)],
                        DateAdded = date,
                        DateRemoved = dateRemoved,
                    }
                    );
                    candidateToStages.Add(
                        new CandidateToStage
                        {
                            Id = candidate.Id.Substring(0, candidate.Id.Length - 3) + vacancyId.Substring(0, 3),
                            StageId = stageId,
                            CandidateId = candidate.Id,
                            MoverId = userIds[random.Next(userIds.Count)],
                            DateAdded = dateRemoved,
                            DateRemoved = null,
                        }
                    );
                }
            }

            return candidateToStages;
        }
    }
}