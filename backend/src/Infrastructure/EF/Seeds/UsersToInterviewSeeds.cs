using System;
using System.Linq;
using System.Collections.Generic;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.EF.Seeds
{
    public static class UsersToInterviewSeeds
    {
        private static Random _random = new Random();

        public static IEnumerable<UsersToInterview> GetUsersTosInterviews()
        {
            IList<UsersToInterview> usersToInterviews = new List<UsersToInterview>();
            foreach (string interviewId in InterviewSeeds.interviewIds)
            {
                var set = new HashSet<string>(VacancySeeds.responsibleHrIds);
                
                for(int i = 0; i < _random.Next(1, VacancySeeds.responsibleHrIds.Count()); i++){
                    var userId = set.ToArray()[_random.Next(set.Count())];
                    usersToInterviews.Add(
                        new UsersToInterview{
                            Id = Guid.NewGuid().ToString(),
                            UserId = userId,
                            InterviewId = interviewId,
                        }
                    );
                    set.Remove(userId);
                }
            }

            return usersToInterviews;
        }
    }
}