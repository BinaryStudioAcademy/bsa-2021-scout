using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.EF.Seeding
{
    public static class ApplicationDbContextSeeding
    {
        public static void Seed(IServiceScope scope)
        {
            using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            SeedRoles(context);
            SeedCompanies(context);
            SeedUsers(context, scope);
        }
        
        private static void SeedRoles(ApplicationDbContext context)
        {
            if (!context.Roles.Any())
            {
                var roles = new List<Role>
                {
                    new Role { Id = "1", Key = 1, Name = "HrLead" },
                    new Role { Id = "2", Key = 2, Name = "HrUser"}
                };
                context.AddRange(roles);
                context.SaveChanges();
            }
        }

        private static void SeedCompanies(ApplicationDbContext context)
        {
            if (!context.Companies.Any())
            {
                var company = new Company() { Id = "1", Name = "Binary Studio", Description = "many words about the company" }; ;
                context.Add(company);
                context.SaveChanges();
            }
        }

        private static void SeedUsers(ApplicationDbContext context, IServiceScope scope)
        {
            if (!context.Users.Any())
            {
                var usersRoles = new List<UserToRole>
                {
                    new UserToRole { UserId = "1", RoleId = "1"},
                    new UserToRole { UserId = "1", RoleId = "2"},
                    new UserToRole { UserId = "2", RoleId = "2"}
                };
                var users = new List<User>
                {
                    new User { Id = "1", FirstName = "Hr", LastName = "Lead", Email = "hrlead@gmail.com", CompanyId = "1", BirthDate = new DateTime(1990, 1, 11) },
                    new User { Id = "2", FirstName = "Dominic", LastName = "Torreto", Email = "family@gmail.com", CompanyId = "1", BirthDate = new DateTime(1976,8, 29) }
                };

                var securityService = scope.ServiceProvider.GetService<ISecurityService>();
                var passwords = new string[] { "hrlead", "family" };
                for (int i = 0; i < users.Count; i++)
                {
                    var salt = securityService.GetRandomBytes();
                    users[i].PasswordSalt = Convert.ToBase64String(salt);
                    users[i].Password = securityService.HashPassword(passwords[i], salt);
                }

                context.AddRange(usersRoles);
                context.AddRange(users);
                context.SaveChanges();
            }
        }
    }
}
