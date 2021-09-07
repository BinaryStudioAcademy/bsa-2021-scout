using Domain.Common;
using Domain.Common.Interfaces;
using System.Collections.Generic;
using Domain.Entities.Abstractions;
using System;

namespace Domain.Entities
{
    public class User : Human, IHasDomainEvent
    {
        public User()
        {
            DomainEvents = new List<DomainEvent>();
        }

        public string AvatarId { get; set; }
        public FileInfo Avatar { get; set; }
        public string Skype { get; set; }
        public string Slack { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string ResetPasswordToken { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public string CompanyId { get; set; }
        public DateTime CreationDate { get; set; }

        public Company Company { get; set; }
        public EmailToken EmailToken { get; set; }
        public ICollection<Vacancy> Vacancies { get; set; }
        public ICollection<VacancyCandidate> AddedCandidates { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
        public ICollection<UserToRole> UserRoles { get; set; }
        public ICollection<UsersToInterview> UsersToInterviews {get; set;}
        public ICollection<CvParsingJob> CvParsingJobs { get; set; }
        public ICollection<SkillsParsingJob> SkillsParsingJobs { get; set; }
        public ICollection<CandidateToStage> MovedCandidateToStages { get; set; }

        public ICollection<UserToTask> UserTask { get; set; }
        public IList<DomainEvent> DomainEvents { get; set; }

    }
}
