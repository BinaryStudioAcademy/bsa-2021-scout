using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.EF.Seeds
{
    public static class UserSeeds
    {
        public static IEnumerable<User> Users { get; } = new List<User>
        {
            new User {
                Id = "1",
                FirstName = "Hr",
                LastName = "Lead",
                Email = "hrlead@gmail.com",
                CompanyId = "1",
                IsEmailConfirmed = true,
                Password = "hrlead",
                BirthDate = new DateTime(1990, 1, 11),
                CreationDate = new DateTime(2019, 3, 4)
            },
            new User {
                Id = "2",
                FirstName = "Dominic",
                LastName = "Torreto",
                Email = "family@gmail.com",
                CompanyId = "1",
                IsEmailConfirmed = true,
                Password = "family",
                BirthDate = new DateTime(1989, 8, 29),
                CreationDate = new DateTime(2020, 5, 20)
            },
            new User{
                Id = "057c23ff-58b1-4531-8012-0b5c1f949ee1",
                FirstName = "Lana",
                LastName = "Winters",
                CompanyId = "1",
                Email = "lanabanana@qmail.com",
                IsEmailConfirmed  = true,
                Password = "ResidentHR",
                BirthDate = new DateTime(2000, 4, 12) ,
                CreationDate = new DateTime(2020, 7, 15)
            },
            new User{
                Id = "fb055f22-c5d1-4611-8e49-32a46edf58b2",
                FirstName = "Lina",
                LastName = "Baptista",
                CompanyId = "1",
                Email = "lina23ista@redsail.org",
                IsEmailConfirmed  = true,
                Password = "Husky2012",
                BirthDate = new DateTime(2002, 11, 12),
                CreationDate = new DateTime(2020, 1, 9)
            },
            new User{
                Id = "ac78dabf-929c-4fa5-9197-7d14e18b40ab",
                FirstName = "Emery",
                LastName = "Culhane",
                CompanyId = "1",
                Email = "emerty.cult@fast.hr",
                IsEmailConfirmed  = true,
                Password = "NewYork2001",
                BirthDate = new DateTime(1994, 4, 14) ,
                CreationDate = new DateTime(2021, 1, 22)
            },
            new User{
                Id = "f8afcbaa-54bd-4103-b3f0-0dd17a5226ca",
                FirstName = "Mira",
                LastName = "Workham",
                CompanyId = "1",
                Email = "miramira@work.done",
                IsEmailConfirmed  = true,
                Password = "WorkDone",
                BirthDate = new DateTime(1993, 2, 23),
                CreationDate = new DateTime(2021, 3, 19)
            },
            new User{
                Id = "8be08dba-5dac-408a-ab6e-8d162af74024",
                FirstName = "John",
                LastName = "Constantine",
                CompanyId = "1",
                Email = "jonny23@dark.art",
                IsEmailConfirmed  = true,
                Password = "DCtheBest",
                BirthDate = new DateTime(1997, 9, 30),
                CreationDate = new DateTime(2020, 6, 16)
            },
            new User{
                Id = "f814400d-2c59-4d58-ad60-da99e99f1c13",
                FirstName = "Brandi",
                LastName = "Green",
                CompanyId = "1",
                Email = "greenpeace@gmail.com",
                IsEmailConfirmed  = false,
                Password = "grEEnpeace12",
                BirthDate = new DateTime(1995, 3, 12),
                CreationDate = new DateTime(2020, 3, 12)
            },
            new User{
                Id = "bcdf5fe4-bc2b-4097-8261-0035d6e579cd",
                FirstName = "Nora",
                LastName = "Bean",
                CompanyId = "1",
                Email = "msbean@gmail.com",
                IsEmailConfirmed  = true,
                Password = "mSBean",
                BirthDate = new DateTime(1992, 5, 21),
                CreationDate = new DateTime(2020, 1, 26)
            },
            new User{
                Id = "2a8b9535-36ac-434d-9c4e-bd03a25935ec",
                FirstName = "Kianna",
                LastName = "Sheppard",
                CompanyId = "1",
                Email = "kiannashepard@gmail.com",
                IsEmailConfirmed  = true,
                Password = "KIannaSHepard",
                BirthDate = new DateTime(1999, 9, 9),
                CreationDate = new DateTime(2021, 2, 5)
            },
            new User{
                Id = "9dd9a3b2-ce2c-4655-86db-ab3c9fb57ae4",
                FirstName = "Najma",
                LastName = "Oneil",
                CompanyId = "1",
                Email = "onenajma@gmail.com",
                IsEmailConfirmed  = false,
                Password = "najma1il",
                BirthDate = new DateTime(1993, 2, 8),
                CreationDate = new DateTime(2020, 7, 5)
            },
            new User{
                Id = "c87c1dd6-addf-4b23-95e8-8836385238b5",
                FirstName = "Brax",
                LastName = "Houghton",
                CompanyId = "1",
                Email = "powerbrax@gmail.com",
                IsEmailConfirmed  = false,
                Password = "powerbraX",
                BirthDate = new DateTime(1998, 11, 3),
                CreationDate = new DateTime(2020, 4, 15)
            },
            new User{
                Id = "a82388a5-b897-4ee9-bbd0-02cea666728d",
                FirstName = "Danyal",
                LastName = "Gregory",
                CompanyId = "1",
                Email = "gregdan@gmail.com",
                IsEmailConfirmed  = true,
                Password = "gregDAN",
                BirthDate = new DateTime(1990, 1, 10),
                CreationDate = new DateTime(2019, 10, 13)
            },
            new User{
                Id = "f97f273e-2d0d-4333-a34f-63acfd55af9b",
                FirstName = "Kaylan",
                LastName = "Colon",
                CompanyId = "1",
                Email = "color20@gmail.com",
                IsEmailConfirmed  = true,
                Password = "yelloW",
                BirthDate = new DateTime(1994, 2, 16),
                CreationDate = new DateTime(2020, 3, 22)
            },
            new User{
                Id = "e5059ae0-8e7e-4656-b350-abc44b241370",
                FirstName = "Maisha",
                LastName = "Madden",
                CompanyId = "1",
                Email = "kastiel@gmail.com",
                IsEmailConfirmed  = false,
                Password = "angelKas",
                BirthDate = new DateTime(1997, 5, 5),
                CreationDate = new DateTime(2021, 4, 1)
            }
        };
    }
}