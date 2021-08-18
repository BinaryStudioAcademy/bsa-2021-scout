
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
            var writeRepository = scope.ServiceProvider.GetService<IReadRepository<MailTemplate>>();
            var connection = connectionFactory.GetMongoConnection();
            var collection = connection.GetCollection<MailTemplate>(typeof(MailTemplate).Name);

            try
            {
                MailTemplate check = writeRepository.GetByPropertyAsync("Slug", "default").Result;
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
            var writeRepo = scope.ServiceProvider.GetService<IWriteRepository<Vacancy>>();
            var readRepo = scope.ServiceProvider.GetService<IReadRepository<Vacancy>>();

            foreach (var vacancy in VacancySeeds.GetVacancies())
            {
                try
                {
                    await readRepo.GetAsync(vacancy.Id);
                    await writeRepo.UpdateAsync(vacancy);
                }
                catch
                {
                    await writeRepo.CreateAsync(vacancy);
                }
            }

            return host;
        }

        public async static Task<IHost> ApplyCompanySeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var writeRepo = scope.ServiceProvider.GetService<IWriteRepository<Company>>();
            var readRepo = scope.ServiceProvider.GetService<IReadRepository<Company>>();

            foreach (var company in CompanySeeds.GetCompanies())
            {
                try
                {
                    await readRepo.GetAsync(company.Id);
                    await writeRepo.UpdateAsync(company);
                }
                catch
                {
                    await writeRepo.CreateAsync(company);
                }

            }

            return host;
        }

        public async static Task<IHost> ApplyProjectSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var writeRepo = scope.ServiceProvider.GetService<IWriteRepository<Project>>();
            var readRepo = scope.ServiceProvider.GetService<IReadRepository<Project>>();

            foreach (var project in ProjectSeeds.GetProjects())
            {
                try
                {
                    await writeRepo.CreateAsync(project);
                }
                catch
                {
                    await writeRepo.UpdateAsync(project);
                }
            }

            return host;
        }

        public async static Task<IHost> ApplyStageSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var writeRepo = scope.ServiceProvider.GetService<IWriteRepository<Stage>>();
            var readRepo = scope.ServiceProvider.GetService<IReadRepository<Stage>>();

            foreach (var stage in StageSeeds.GetStages())
            {
                try
                {
                    await readRepo.GetAsync(stage.Id);
                    await writeRepo.UpdateAsync(stage);
                }
                catch
                {
                    await writeRepo.CreateAsync(stage);
                }
            }

            return host;
        }

        public async static Task<IHost> ApplyUserSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var writeRepo = scope.ServiceProvider.GetService<IWriteRepository<User>>();
            var readRepo = scope.ServiceProvider.GetService<IReadRepository<User>>();

            foreach (var user in UserSeeds.GetUsers())
            {
                try
                {
                    await readRepo.GetAsync(user.Id);
                    await writeRepo.UpdateAsync(user);
                }
                catch
                {
                    await writeRepo.CreateAsync(user);
                }
            }

            return host;
        }

        public async static Task<IHost> ApplyReviewSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var writeRepo = scope.ServiceProvider.GetService<IWriteRepository<Review>>();
            var readRepo = scope.ServiceProvider.GetService<IReadRepository<Review>>();

            foreach (var review in ReviewSeeds.GetReviews())
            {
                try
                {
                    await readRepo.GetAsync(review.Id);
                    await writeRepo.UpdateAsync(review);
                }
                catch
                {
                    await writeRepo.CreateAsync(review);
                }
            }

            return host;
        }

        public async static Task<IHost> ApplyReviewToStageSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var writeRepo = scope.ServiceProvider.GetService<IWriteRepository<ReviewToStage>>();
            var readRepo = scope.ServiceProvider.GetService<IReadRepository<ReviewToStage>>();

            foreach (var reviewToStage in ReviewToStageSeeds.GetReviewToStages())
            {
                try
                {
                    await readRepo.GetAsync(reviewToStage.Id);
                    await writeRepo.UpdateAsync(reviewToStage);
                }
                catch
                {
                    await writeRepo.CreateAsync(reviewToStage);
                }
            }

            return host;
        }

        public async static Task<IHost> ApplyGeneralDatabaseSeeding(this IHost host)
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

        public static async Task<IHost> ApplySqlSeeding(this IHost host)
        {
            await ApplyCompanySeeding(host);
            await ApplyGeneralDatabaseSeeding(host);
            await ApplyProjectSeeding(host);
            await ApplyUserSeeding(host);
            await ApplyVacancySeeding(host);
            await ApplyStageSeeding(host);
            await ApplyReviewSeeding(host);
            await ApplyReviewToStageSeeding(host);

            return host;
        }

        public static IHost ApplyAllSeedingSync(this IHost host)
        {
            ApplyMongoSeeding(host);
            ApplyElasticSeeding(host);
            ApplySqlSeeding(host).Wait();

            return host;
        }
    }
}
