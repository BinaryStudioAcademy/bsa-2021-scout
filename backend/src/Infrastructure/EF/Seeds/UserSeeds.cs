using System;
using System.Collections.Generic;
using Domain.Entities;

namespace Infrastructure.EF.Seeds
{
    public static class UserSeeds
    {
        private static Random _random = new Random();

        public static IEnumerable<User> GetUsers()
        {
            DateTime birthDate = DateTime.Now.AddYears(-20);

            return new List<User>
            {
                new User {
                    Id = "057c23ff-58b1-4531-8012-0b5c1f949ee1",
                    FirstName = "Lana",
                    LastName = "Winters",
                    BirthDate = birthDate,
                    Email = "lw@gmail.com",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                },
                new User {
                    Id = "fb055f22-c5d1-4611-8e49-32a46edf58b2",
                    FirstName = "Lina",
                    LastName = "Baptista",
                    BirthDate = birthDate,
                    Email = "lb@yahoo.com",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                },
                new User {
                    Id = "ac78dabf-929c-4fa5-9197-7d14e18b40ab",
                    FirstName = "Emery",
                    LastName = "Culhane",
                    BirthDate = birthDate,
                    Email = "ec@gmail.com",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                },
                new User {
                    Id = "f8afcbaa-54bd-4103-b3f0-0dd17a5226ca",
                    FirstName = "Mira",
                    LastName = "Workham",
                    BirthDate = birthDate,
                    Email = "mw@gmail.com",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                },
                new User {
                    Id = "8be08dba-5dac-408a-ab6e-8d162af74024",
                    FirstName = "John",
                    LastName = "Constantine",
                    BirthDate = birthDate,
                    Email = "jc@gmail.com",
                    CompanyId = "0b129ab3-7375-4c96-95a5-8efa95a455b4",
                }
            };
        }
    }
}