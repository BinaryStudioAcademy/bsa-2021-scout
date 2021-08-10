using System;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.EF.Seeds
{
    public static class VacancySeeds
    {
        private static Random _random = new Random();
        public static DateTime GetRandomDateTime(int daysOffset = 0){
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days - daysOffset;
            return start.AddDays(VacancySeeds._random.Next(range));
        }
        public static IEnumerable<Vacancy> Vacancies { get; } = new List<Vacancy>
        {
            new Vacancy
            {
                Id = "075eb5a4-c37d-4706-b325-c9ca8ed7ad83",
                Title = "Developer",
                Requirements = "¯\\_(ツ)_/¯",
                Status = Domain.Enums.VacancyStatus.Active,
                CreationDate = GetRandomDateTime(30),
                DateOfOpening = GetRandomDateTime(-10),
                ModificationDate = GetRandomDateTime(2),
                IsRemote = true,
                IsHot = false,
                SalaryFrom = 13432,
                SalaryTo = 34002,
                CompletionDate = GetRandomDateTime(-29),
                PlannedCompletionDate = GetRandomDateTime(-30),
                TierFrom = Tier.Middle,
                TierTo = Tier.TeamLead,
                Sources = "¯\\_(ツ)_/¯",
                
                
                
            },
            new Vacancy
            {
                Id = "7f43b573-d28f-4689-bbb2-7924c8321121",
                Title = "Software Enginner",
                Requirements = "¯\\_(ツ)_/¯",
                Status = Domain.Enums.VacancyStatus.Former,
                CreationDate = GetRandomDateTime(30),
                DateOfOpening = GetRandomDateTime(-10),
                ModificationDate = GetRandomDateTime(2),
                IsRemote = false,
                IsHot = false,
                SalaryFrom = 1000,
                SalaryTo = 3000,
                CompletionDate = GetRandomDateTime(-29),
                PlannedCompletionDate = GetRandomDateTime(-30),
                TierFrom = Tier.Junior,
                TierTo = Tier.Middle,
                Sources = "¯\\_(ツ)_/¯",
                
                
                
            },
            new Vacancy
            {
                Id = "64cb91e7-2ea0-4b91-8484-b441969ea67e",
                Title = "QA",
                Requirements = "¯\\_(ツ)_/¯",
                Status = Domain.Enums.VacancyStatus.Invited,
                CreationDate = GetRandomDateTime(30),
                DateOfOpening = GetRandomDateTime(-10),
                ModificationDate = GetRandomDateTime(2),
                IsRemote = false,
                IsHot = true,
                SalaryFrom = 10,
                SalaryTo = 3203,
                CompletionDate = GetRandomDateTime(-29),
                PlannedCompletionDate = GetRandomDateTime(-30),
                TierFrom = Tier.Junior,
                TierTo = Tier.Middle,
                Sources = "¯\\_(ツ)_/¯",
                
                
                
            },
            new Vacancy
            {
                Id = "ac57b3cd-daab-4cce-82d1-cf184f425166",
                Title = "Project Manager",
                Requirements = "¯\\_(ツ)_/¯",
                Status = Domain.Enums.VacancyStatus.Vacation,
                CreationDate = GetRandomDateTime(30),
                DateOfOpening = GetRandomDateTime(-10),
                ModificationDate = GetRandomDateTime(2),
                IsRemote = true,
                IsHot = true,
                SalaryFrom = 3000,
                SalaryTo = 9000,
                CompletionDate = GetRandomDateTime(-29),
                PlannedCompletionDate = GetRandomDateTime(-30),
                TierFrom = Tier.Middle,
                TierTo = Tier.Middle,
                Sources = "¯\\_(ツ)_/¯",
                
                
                
            },
            new Vacancy
            {
                Id = "a7d1bdb6-57c9-436d-baf7-b79d0c88adcf",
                Title = "Interface Designer",
                Requirements = "¯\\_(ツ)_/¯",
                Status = Domain.Enums.VacancyStatus.Vacation,
                CreationDate = GetRandomDateTime(30),
                DateOfOpening = GetRandomDateTime(-10),
                ModificationDate = GetRandomDateTime(2),
                IsRemote = false,
                IsHot = false,
                SalaryFrom = 13000,
                SalaryTo = 33000,
                CompletionDate = GetRandomDateTime(-29),
                PlannedCompletionDate = GetRandomDateTime(-30),
                TierFrom = Tier.Senior,
                TierTo = Tier.TeamLead,
                Sources = "¯\\_(ツ)_/¯",
                
                
                
            },
        };
    }
}