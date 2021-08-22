using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.EF.Seeds
{
    public static class ApplicantSeeds
    {
        public static IEnumerable<Applicant> GetApplicants()
        {
            return new List<Applicant>
            {
                new Applicant{
                    Id = "732f84f8-0625-5098-adbf-4aabcabb5b0c",
                    FirstName = "Tyler",
                    LastName = "Howard",
                    LinkedInUrl = "http://linkedin.com/copid",
                    Email = "mozan@saz.gh",
                    Phone = "3108168766",
                    ToBeContacted = new DateTime(2021, 09, 23),
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
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
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
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
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
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
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
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
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Experience = 1.3,
                    Skype = "http://skype.com/jienkas",
                },
                new Applicant{
                    Id = "8be38dd0-8767-54c5-8f78-aa33e11997e9",
                    FirstName = "Clarence",
                    LastName = "Hoffman",
                    LinkedInUrl = "http://linkedin.com/piwik",
                    Email = "zikawitid@ki.bb",
                    Phone = "5336301179",
                    ToBeContacted = new DateTime(2021, 11, 09),
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Experience = 5,
                    Skype = "http://skype.com/piwik",
                },
                // 
                new Applicant{
                    Id = "c8698455-0a05-503f-a6a6-b8557093a67e",
                    FirstName = "Scott",
                    LastName = "Horton",
                    LinkedInUrl = "http://linkedin.com/imeupifom",
                    Email = "vomna@su.mp",
                    Phone = "4209938662",
                    ToBeContacted = new DateTime(2021, 11, 12),
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Experience = 1.2,
                    Skype = "http://skype.com/imeupifom",
                },
                new Applicant{
                    Id = "32e88c57-7a44-51fa-abeb-b9253dacc14f",
                    FirstName = "Mark",
                    LastName = "Mullins",
                    LinkedInUrl = "http://linkedin.com/imcudkam",
                    Email = "zeb@toceb.af",
                    Phone = "9375884723",
                    ToBeContacted = new DateTime(2021, 11, 13),
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Experience = 1.3,
                    Skype = "http://skype.com/imcudkam",
                },
                new Applicant{
                    Id = "6a9e4cf9-1a02-50dd-9d32-d82fa566c5a2",
                    FirstName = "Isabelle",
                    LastName = "Baldwin",
                    LinkedInUrl = "http://linkedin.com/fanforom",
                    Email = "ideb@aro.kp",
                    Phone = "5246769909",
                    ToBeContacted = new DateTime(2021, 11, 14),
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Experience = 1.5,
                    Skype = "http://skype.com/fanforom",
                },
                new Applicant{
                    Id = "5f08c0b7-e605-5672-acce-328b8d660989",
                    FirstName = "Alejandro",
                    LastName = "Ramirez",
                    LinkedInUrl = "http://linkedin.com/ubkiz",
                    Email = "rudruvnu@liku.es",
                    Phone = "6088654301",
                    ToBeContacted = new DateTime(2021, 12, 15),
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Experience = 2.4,
                    Skype = "http://skype.com/ubkiz",
                },
                new Applicant{
                    Id = "342f8205-6c3f-5b33-8148-bf6fa9ebda64",
                    FirstName = "Edward",
                    LastName = "Banks",
                    LinkedInUrl = "http://linkedin.com/zehipu",
                    Email = "macfus@rejokovoh.bg",
                    Phone = "9467574233",
                    ToBeContacted = new DateTime(2021, 10, 11),
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Experience = 1.6,
                    Skype = "http://skype.com/zehipu",
                },
                new Applicant{
                    Id = "8f906dbd-f7dd-54fc-b59f-17d7b1a153dc",
                    FirstName = "Sylvia",
                    LastName = "Butler",
                    LinkedInUrl = "http://linkedin.com/cemketrir",
                    Email = "doapi@rerug.bf",
                    Phone = "2542288391",
                    ToBeContacted = new DateTime(2021, 10, 24),
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Experience = 4.3,
                    Skype = "http://skype.com/cemketrir",
                },
                new Applicant{
                    Id = "eec0eb4b-1999-5b4a-8dad-5f41c8b3e83c",
                    FirstName = "Margaret",
                    LastName = "Hopkins",
                    LinkedInUrl = "http://linkedin.com/kalafhig",
                    Email = "pawusuel@fozhaf.ss",
                    Phone = "3098489401",
                    ToBeContacted = new DateTime(2021, 11, 23),
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Experience = 3.7,
                    Skype = "http://skype.com/kalafhig",
                },
                new Applicant{
                    Id = "388b41ec-993a-5196-9dee-fe831257dda7",
                    FirstName = "Beulah",
                    LastName = "Martinez",
                    LinkedInUrl = "http://linkedin.com/sacaksi",
                    Email = "guira@giwa.cu",
                    Phone = "9566854321",
                    ToBeContacted = new DateTime(2021, 11, 9),
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Experience = 2.8,
                    Skype = "http://skype.com/sacaksi",
                },
                new Applicant{
                    Id = "d6f8a112-c9bb-5e0e-a960-53a9da2495ab",
                    FirstName = "Curtis",
                    LastName = "Allen",
                    LinkedInUrl = "http://linkedin.com/buafso",
                    Email = "wicgalel@enidibze.tt",
                    Phone = "3013118526",
                    ToBeContacted = new DateTime(2021, 11, 7),
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Experience = 4.6,
                    Skype = "http://skype.com/buafso",
                },
                new Applicant{
                    Id = "eccf7842-db64-5e70-b010-67cb3806f2e4",
                    FirstName = "Edward",
                    LastName = "Adams",
                    LinkedInUrl = "http://linkedin.com/ubese",
                    Email = "fedef@sanzu.ke",
                    Phone = "4063224052",
                    ToBeContacted = new DateTime(2021, 11, 6),
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Experience = 5.6,
                    Skype = "http://skype.com/ubese",
                },
                new Applicant{
                    Id = "aa2d6886-ef45-5d0e-a2fa-4c839f5f6e99",
                    FirstName = "Bernard",
                    LastName = "Gray",
                    LinkedInUrl = "http://linkedin.com/kobenufi",
                    Email = "vicidanom@etefilwih.az",
                    Phone = "9784361088",
                    ToBeContacted = new DateTime(2021, 11, 2),
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Experience = 1.9,
                    Skype = "http://skype.com/kobenufi",
                },
                new Applicant{
                    Id = "98954928-1256-557a-b79f-7058641f0b4b",
                    FirstName = "Ryan",
                    LastName = "Watts",
                    LinkedInUrl = "http://linkedin.com/gecap",
                    Email = "buwgo@kivjowdi.us",
                    Phone = "4366164414",
                    ToBeContacted = new DateTime(2021, 10, 3),
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Experience = 3.7,
                    Skype = "http://skype.com/gecap",
                },
                new Applicant{
                    Id = "74c1dac0-d4bb-51d9-a2e2-647952974ad6",
                    FirstName = "Derek",
                    LastName = "Morrison",
                    LinkedInUrl = "http://linkedin.com/gitebiv",
                    Email = "gehag@fuwgama.st",
                    Phone = "7492258530",
                    ToBeContacted = new DateTime(2021, 10, 4),
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Experience = 3.6,
                    Skype = "http://skype.com/gitebiv",
                },
                new Applicant{
                    Id = "91f8c039-6798-5692-9bb9-68bc16bc4a82",
                    FirstName = "Carrie",
                    LastName = "Santos",
                    LinkedInUrl = "http://linkedin.com/puberun",
                    Email = "mubone@uru.ae",
                    Phone = "6194343848",
                    ToBeContacted = new DateTime(2021, 11, 10),
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Experience = 2.6,
                    Skype = "http://skype.com/puberun",
                },
            };
        }
    }
}