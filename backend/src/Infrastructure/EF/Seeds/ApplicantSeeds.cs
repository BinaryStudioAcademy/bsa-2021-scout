using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.EF.Seeds
{
    public static class ApplicantSeeds
    {
        public static IEnumerable<Applicant> Applicants { get; } = new List<Applicant>
        {
            new Applicant{
                Id = "732f84f8-0625-5098-adbf-4aabcabb5b0c",
                FirstName = "Tyler",
                LastName = "Howard",
                LinkedInUrl = "http://linkedin.com/copid",
                Email = "mozan@saz.gh",
                Phone = "3108168766",
                ToBeContacted = new DateTime(2021, 09, 23),
                CompanyId = "1",
                Experience = 2.4,
                Skype = "http://skype.com/copid",
            },
             new Applicant{ 
                Id = "bffcc629-77ef-5304-9d4a-d9b44b5739d6",
                FirstName = "Gregory",
                LastName = "Andrews",
                LinkedInUrl = "http://linkedin.com/acsippid",
                Email = "dervasla@ne.eu",
                Phone = "9414652442",
                ToBeContacted = new DateTime(2021, 12, 23),
                CompanyId = "1",
                Experience = 1.9,
                Skype = "http://skype.com/acsippid",
            },
             new Applicant{ 
                Id = "4e5d00dc-f1ab-5a74-9e6f-4edae70fca02",
                FirstName = "Alex", 
                LastName = "Waters",
                LinkedInUrl = "http://linkedin.com/nedvavjed",
                Email = "do@narvum.pg",
                Phone = "4514957183",
                ToBeContacted = new DateTime(2021, 10, 12),
                CompanyId = "1",
                Experience = 3.4,
                Skype = "http://skype.com/nedvavjed",
            },
             new Applicant{ 
                Id = "3e84df9f-f6c3-50d8-8787-d0e2a94af2b6",
                FirstName = "Dollie",
                LastName = "Warren",
                LinkedInUrl = "http://linkedin.com/riclojalo",
                Email = "su@kebizo.re",
                Phone = "3103016154", 
                ToBeContacted = new DateTime(2021, 11, 03),
                CompanyId = "1",
                Experience = 2.3,
                Skype = "http://skype.com/riclojalo",
            },
             new Applicant{ 
                Id = "8bf07b5b-af46-58be-b536-58eeca69f661",
                FirstName = "Robert", 
                LastName = "Swanson",
                LinkedInUrl = "http://linkedin.com/jienkas",
                Email = "zimhapi@zicpam.bj",
                Phone = "4003459295",
                ToBeContacted = new DateTime(2021, 09, 06),
                CompanyId = "1",
                Experience = 1.3,
                Skype = "http://skype.com/jienkas",
            },
             new Applicant{ 
                Id = "8be38dd0-8767-54c5-8f78-aa33e11997e9",
                FirstName = "Clarence", 
                LastName = "Hoffman",
                LinkedInUrl = "http://linkedin.com/piwik",
                Email = "dervasla@ne.eu",
                Phone = "5336301179",
                ToBeContacted = new DateTime(2021, 11, 09),
                CompanyId = "1",
                Experience = 5,
                Skype = "http://skype.com/piwik",
            },
        };
    }
}