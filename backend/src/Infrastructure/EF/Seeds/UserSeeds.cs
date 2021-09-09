using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.EF.Seeds
{
    public static class UserSeeds
    {
        public static IEnumerable<User> GetUsers()
        {
            return new List<User>
            {
                new User {
                    Id = "ba5073cc-4322-483d-b69a-f43b388091e9",
                    FirstName = "Mark",
                    LastName = "Twain",
                    Email = "scouttest@ukr.net",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    IsEmailConfirmed = true,
                    Password = "scouttest",
                    CreationDate =  Common.GetRandomDateTime(new DateTime(2020, 1, 12), new DateTime(2021, 7, 12)),
                    BirthDate = new DateTime(1998, 9, 11)
                },
                new User {
                    Id = "1",
                    FirstName = "Hr",
                    LastName = "Lead",
                    Email = "hrlead@gmail.com",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    IsEmailConfirmed = true,
                    Password = "hrlead",
                    CreationDate = Common.GetRandomDateTime(new DateTime(2020, 1, 12), new DateTime(2021, 7, 12)),
                    BirthDate = new DateTime(1990, 1, 11)
                },
                new User {
                    Id = "2",
                    FirstName = "Dominic",
                    LastName = "Torreto",
                    Email = "family@gmail.com",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    IsEmailConfirmed = true,
                    Password = "family",
                    CreationDate = Common.GetRandomDateTime(new DateTime(2020, 1, 12), new DateTime(2021, 7, 12)),
                    BirthDate = new DateTime(1989, 8, 29)
                },
                new User {
                    Id = "057c23ff-58b1-4531-8012-0b5c1f949ee1",
                    FirstName = "Lana",
                    LastName = "Winters",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Email = "lanabanana@qmail.com",
                    IsEmailConfirmed  = true,
                    Password = "ResidentHR",
                    CreationDate = Common.GetRandomDateTime(new DateTime(2020, 1, 12), new DateTime(2021, 7, 12)),
                    BirthDate = new DateTime(2000, 4, 12)
                },
                new User {
                    Id = "fb055f22-c5d1-4611-8e49-32a46edf58b2",
                    FirstName = "Lina",
                    LastName = "Baptista",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Email = "lina23ista@redsail.org",
                    IsEmailConfirmed  = true,
                    Password = "Husky2012",
                    CreationDate = Common.GetRandomDateTime(new DateTime(2020, 1, 12), new DateTime(2021, 7, 12)),
                    BirthDate = new DateTime(2002, 11, 12)
                },
                new User {
                    Id = "ac78dabf-929c-4fa5-9197-7d14e18b40ab",
                    FirstName = "Emery",
                    LastName = "Culhane",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Email = "emerty.cult@fast.hr",
                    IsEmailConfirmed  = true,
                    Password = "NewYork2001",
                    CreationDate = Common.GetRandomDateTime(new DateTime(2020, 1, 12), new DateTime(2021, 7, 12)),
                    BirthDate = new DateTime(1994, 4, 14)
                },
                new User {
                    Id = "f8afcbaa-54bd-4103-b3f0-0dd17a5226ca",
                    FirstName = "Mira",
                    LastName = "Workham",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Email = "miramira@work.done",
                    IsEmailConfirmed  = true,
                    Password = "WorkDone",
                    CreationDate = Common.GetRandomDateTime(new DateTime(2020, 1, 12), new DateTime(2021, 7, 12)),
                    BirthDate = new DateTime(1993, 2, 23)
                },
                new User {
                    Id = "8be08dba-5dac-408a-ab6e-8d162af74024",
                    FirstName = "John",
                    LastName = "Constantine",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                    Email = "jonny23@dark.art",
                    IsEmailConfirmed  = true,
                    Password = "DCtheBest",
                    CreationDate = Common.GetRandomDateTime(new DateTime(2020, 1, 12), new DateTime(2021, 7, 12)),
                    BirthDate = new DateTime(1997, 9, 30)
                }
            };
        }
    }
}