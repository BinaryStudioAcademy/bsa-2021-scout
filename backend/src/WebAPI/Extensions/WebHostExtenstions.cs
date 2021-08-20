
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Infrastructure.EF;
using Infrastructure.EF.Seeds;

using Infrastructure.Mongo.Interfaces;
using Infrastructure.Mongo.Seeding;
using Infrastructure.Elastic.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nest;
using System.Collections.Generic;
using System;
using Application.Interfaces;

namespace WebAPI.Extensions
{
    public static class WebHostExtenstions
    {
        public static IHost ApplyDatabaseMigrations(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Database.Migrate();

            return host;
        }
        public static IHost ApplyElasticSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var client = scope.ServiceProvider.GetService<IElasticClient>();

            client.IndexMany<ElasticEntity>(
                ApplicantTagsSeeds.GetSeed()
            );

            return host;
        }

        public static IHost ApplyMongoSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var connectionFactory = scope.ServiceProvider.GetService<IMongoConnectionFactory>();
            var repository = scope.ServiceProvider.GetService<IReadRepository<MailTemplate>>();
            var connection = connectionFactory.GetMongoConnection();
            var collection = connection.GetCollection<MailTemplate>(typeof(MailTemplate).Name);

            try
            {
                MailTemplate check = repository.GetByPropertyAsync("Slug", "default").Result;
            }
            catch
            {
                collection.InsertOne(MailTemplatesSeeds.GetSeed());
            }

            return host;
        }

        public async static Task<IHost> ApplyVacancySeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var repo = scope.ServiceProvider.GetService<IWriteRepository<Vacancy>>();
            var otherRepo = scope.ServiceProvider.GetService<IReadRepository<Vacancy>>();
            foreach (var vacancy in (new VacancySeeds()).Vacancies())
            {
                try
                {
                    await otherRepo.GetAsync(vacancy.Id);
                    await repo.UpdateAsync(vacancy);
                }
                catch
                {
                    await repo.CreateAsync(vacancy);
                }
            }

            return host;
        }
        public async static Task<IHost> ApplyCompanySeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var repo = scope.ServiceProvider.GetService<IWriteRepository<Company>>();
            var otherRepo = scope.ServiceProvider.GetService<IReadRepository<Company>>();
            foreach (var company in CompanySeeds.Companies)
            {
                try
                {
                    await otherRepo.GetAsync(company.Id);
                    await repo.UpdateAsync(company);
                }
                catch
                {
                    await repo.CreateAsync(company);
                }

            }

            return host;
        }
        public async static Task<IHost> ApplyProjectSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var repo = scope.ServiceProvider.GetService<IWriteRepository<Project>>();
            var otherRepo = scope.ServiceProvider.GetService<IReadRepository<Project>>();
            foreach (var project in ProjectSeeds.Projects)
            {
                try
                {
                    await repo.CreateAsync(project);
                }
                catch
                {
                    await repo.UpdateAsync(project);
                }
            }

            return host;
        }
        public async static Task<IHost> ApplyStageSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var repo = scope.ServiceProvider.GetService<IWriteRepository<Stage>>();
            var otherRepo = scope.ServiceProvider.GetService<IReadRepository<Stage>>();
            foreach (var stage in StageSeeds.Stages())
            {
                try
                {
                    await otherRepo.GetAsync(stage.Id);
                    await repo.UpdateAsync(stage);
                }
                catch
                {
                    await repo.CreateAsync(stage);
                }
            }

            return host;
        }
        public async static Task<IHost> ApplyUserSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var repo = scope.ServiceProvider.GetService<IWriteRepository<User>>();
            var otherRepo = scope.ServiceProvider.GetService<IReadRepository<User>>();
            foreach (var user in UserSeeds.Users)
            {
                try
                {
                    await otherRepo.GetAsync(user.Id);
                    await repo.UpdateAsync(user);
                }
                catch
                {
                    await repo.CreateAsync(user);
                }
            }
            return host;
        }

        public async static Task<IHost> ApplyDatabaseSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var roleWriteRepo = scope.ServiceProvider.GetService<IWriteRepository<Role>>();
            var roleReadRepo = scope.ServiceProvider.GetService<IReadRepository<Role>>();
            var roles = new List<Role>
                {
                    new Role { Id = "1", Key = 1, Name = "HrLead" },
                    new Role { Id = "2", Key = 2, Name = "HrUser"}
                };
            foreach (var role in roles)
            {
                try
                {
                    await roleReadRepo.GetAsync(role.Id);
                }
                catch
                {
                    await roleWriteRepo.CreateAsync(role);
                }
            }
            using var scope2 = host.Services.CreateScope();
            var userWriteRepo = scope2.ServiceProvider.GetService<IWriteRepository<User>>();
            var userReadRepo = scope2.ServiceProvider.GetService<IReadRepository<User>>();
            var users = new List<User>
            {
                new User { Id = "1", FirstName = "Hr", LastName = "Lead", Email = "hrlead@gmail.com", CompanyId = "1", IsEmailConfirmed = true, BirthDate = new DateTime(1990, 1, 11) },
                new User { Id = "2", FirstName = "Dominic", LastName = "Torreto", Email = "family@gmail.com", CompanyId = "1", IsEmailConfirmed = true, BirthDate = new DateTime(1976,8, 29) }
            };
            var securityService = scope.ServiceProvider.GetService<ISecurityService>();
            var passwords = new string[] { "hrlead", "family" };
            for (int i = 0; i < users.Count; i++)
            {
                var salt = securityService.GetRandomBytes();
                users[i].PasswordSalt = Convert.ToBase64String(salt);
                users[i].Password = securityService.HashPassword(passwords[i], salt);
                try
                {
                    await userReadRepo.GetAsync(users[i].Id);
                    await userWriteRepo.UpdateAsync(users[i]);
                }
                catch
                {
                    await userWriteRepo.CreateAsync(users[i]);
                }
            }

            var usersRoles = new List<UserToRole>
            {
                new UserToRole { UserId = "1", RoleId = "1"},
                new UserToRole { UserId = "1", RoleId = "2"},
                new UserToRole { UserId = "2", RoleId = "2"}
            };
            using var scope3 = host.Services.CreateScope();
            var userRoleWriteRepo = scope3.ServiceProvider.GetService<IWriteRepository<UserToRole>>();
            var userRoleReadRepo = scope3.ServiceProvider.GetService<IReadRepository<UserToRole>>();
            foreach (var userRole in usersRoles)
            {
                try
                {
                    await userRoleReadRepo.GetAsync(userRole.Id);
                }
                catch
                {
                    await userRoleWriteRepo.CreateAsync(userRole);
                }
            }
            return host;
        }
    }
}
