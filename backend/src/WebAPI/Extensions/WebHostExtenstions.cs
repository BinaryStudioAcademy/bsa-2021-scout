
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
        public static IHost ApplyAllSeedingSync(this IHost host)
        {
            ApplyAllSeeding(host).Wait();

            return host;
        }

        public static async Task<IHost> ApplyAllSeeding(this IHost host)
        {
            await ApplyMongoSeeding(host);
            await ApplyCompanySeeding(host);
            await ApplyElasticSeeding(host);
            await ApplyRoleSeeding(host);
            await ApplyProjectSeeding(host);
            await ApplyUserSeeding(host);
            await ApplyPoolSeeding(host);
            await ApplyUserToRoleSeeding(host);
            await ApplyVacancySeeding(host);
            await ApplyApplicantSeeding(host);
            await ApplyPoolToApplicantSeeding(host);
            await ApplyStageSeeding(host);
            await ApplyVacancyCandidateSeeding(host);
            await ApplyCandidateToStagesSeeding(host);
            await ApplyReviewSeeding(host);
            await ApplyToDoTaskSeeding(host);
            await ApplyUserToTaskSeeding(host);
            await ApplyInterviewSeeding(host);
            await ApplyUsersToInterviews(host);


            return host;
        }
        public static IHost ApplyDatabaseMigrations(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Database.Migrate();

            return host;
        }
        public static async Task<IHost> ApplyElasticSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var client = scope.ServiceProvider.GetService<IElasticClient>();

            await client.DeleteManyAsync<ElasticEntity>(
                ElasticTagsSeeds.GetSeed()
            );
            await client.IndexManyAsync<ElasticEntity>(
                ElasticTagsSeeds.GetSeed()
            );

            return host;
        }

        public static async Task<IHost> ApplyMongoSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var readRepository = scope.ServiceProvider.GetService<IReadRepository<MailTemplate>>();
            var writeRepository = scope.ServiceProvider.GetService<IWriteRepository<MailTemplate>>();

            var mailTemplates = await readRepository.GetEnumerableAsync();
            var defaultMailTemplates = mailTemplates.Where(x => x.Id == "6130f7b9fd538a80c19ed51e" || x.Slug == "default");
            if (mailTemplates != null && defaultMailTemplates.Count() != 0)
            {
                foreach (var mailTemplate in defaultMailTemplates)
                {
                    await writeRepository.DeleteAsync(mailTemplate.Id);
                }
            }
            await writeRepository.CreateAsync(MailTemplatesSeeds.GetDefaultSeed());

            foreach (var mailTemplate in MailTemplatesSeeds.GetSeeds())
            {
                if (!mailTemplates.Any(x => x.Id == mailTemplate.Id))
                {
                    await writeRepository.CreateAsync(mailTemplate);
                }
            }
            return host;
        }

        public async static Task<IHost> ApplyPoolSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            foreach (var pool in PoolSeeds.Pools)
            {
                if (await context.Pools.AnyAsync(c => c.Id == pool.Id))
                    context.Pools.Update(pool);
                else
                    await context.Pools.AddAsync(pool);
                await context.SaveChangesAsync();
            }

            return host;
        }
        public async static Task<IHost> ApplyPoolToApplicantSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            foreach (var poolToApplicant in PoolToApplicantSeeds.PoolToApplicants)
            {
                if (await context.PoolToApplicants.AnyAsync(c => c.Id == poolToApplicant.Id))
                    context.PoolToApplicants.Update(poolToApplicant);
                else
                    await context.PoolToApplicants.AddAsync(poolToApplicant);
                await context.SaveChangesAsync();
            }

            return host;
        }
        public async static Task<IHost> ApplyCandidateToStagesSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.CandidateToStages.RemoveRange(context.CandidateToStages);
            await context.SaveChangesAsync();
            foreach (var candidateToStage in CandidateToStagesSeeds.CandidateToStages())
            {
                if (await context.CandidateToStages.AnyAsync(c => c.Id == candidateToStage.Id))
                    context.CandidateToStages.Update(candidateToStage);
                else
                    await context.CandidateToStages.AddAsync(candidateToStage);
                await context.SaveChangesAsync();
            }

            return host;
        }
        public async static Task<IHost> ApplyVacancyCandidateSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            foreach (var vacancyCandidate in VacancyCandidateSeeds.VacancyCandidates)
            {
                if (await context.VacancyCandidates.AnyAsync(c => c.Id == vacancyCandidate.Id))
                    context.VacancyCandidates.Update(vacancyCandidate);
                else
                    await context.VacancyCandidates.AddAsync(vacancyCandidate);
                await context.SaveChangesAsync();
            }

            return host;
        }

        public async static Task<IHost> ApplyVacancySeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            foreach (var vacancy in VacancySeeds.GetVacancies())
            {
                if (await context.Vacancies.AnyAsync(c => c.Id == vacancy.Id))
                    context.Vacancies.Update(vacancy);
                else
                    await context.Vacancies.AddAsync(vacancy);
                await context.SaveChangesAsync();
            }

            return host;
        }

        public async static Task<IHost> ApplyCompanySeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            foreach (var company in CompanySeeds.GetCompanies())
            {
                if (await context.Companies.AnyAsync(c => c.Id == company.Id))
                    context.Companies.Update(company);
                else
                    await context.Companies.AddAsync(company);
                await context.SaveChangesAsync();
            }

            return host;
        }

        public async static Task<IHost> ApplyProjectSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.ArchivedEntities.RemoveRange(context.ArchivedEntities);
            await context.SaveChangesAsync();

            foreach (var project in ProjectSeeds.GetProjects())
            {
                if (await context.Projects.AnyAsync(c => c.Id == project.Id))
                    context.Projects.Update(project);
                else
                    await context.Projects.AddAsync(project);
                await context.SaveChangesAsync();
            }

            return host;
        }

        public async static Task<IHost> ApplyStageSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            foreach (var stage in StageSeeds.GetStages())
            {
                if (await context.Stages.AnyAsync(c => c.Id == stage.Id))
                    context.Stages.Update(stage);
                else
                    await context.Stages.AddAsync(stage);
                await context.SaveChangesAsync();
            }

            return host;
        }
        public async static Task<IHost> ApplyApplicantSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            foreach (var applicant in ApplicantSeeds.GetApplicants())
            {
                if (await context.Applicants.AnyAsync(c => c.Id == applicant.Id))
                    context.Applicants.Update(applicant);
                else
                    await context.Applicants.AddAsync(applicant);
                await context.SaveChangesAsync();
            }

            return host;
        }
        public async static Task<IHost> ApplyUserSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var securityService = scope.ServiceProvider.GetService<ISecurityService>();
            foreach (var user in UserSeeds.GetUsers())
            {
                var salt = securityService.GetRandomBytes();
                user.PasswordSalt = Convert.ToBase64String(salt);
                user.Password = securityService.HashPassword(user.Password, salt);
                if (await context.Users.AnyAsync(c => c.Id == user.Id))
                    context.Users.Update(user);
                else
                    await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
            }
            return host;
        }
        public async static Task<IHost> ApplyUserToRoleSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.UserToRoles.RemoveRange(context.UserToRoles);
            var usersToRoles = new List<UserToRole>
            {
                new UserToRole { UserId = "ba5073cc-4322-483d-b69a-f43b388091e9", RoleId = "1"},
                new UserToRole { UserId = "1", RoleId = "1"},
                new UserToRole { UserId = "1", RoleId = "2"},
            };

            Random random = new Random();
            foreach (var user in UserSeeds.GetUsers().Skip(2))
            {
                usersToRoles.Add(
                new UserToRole
                {
                    UserId = user.Id,
                    RoleId = RoleSeeds.Roles[random.Next(RoleSeeds.Roles.Count())].Id
                });
            }
            foreach (var userToRole in usersToRoles)
            {
                if (await context.UserToRoles.AnyAsync(c => c.Id == userToRole.Id))
                    context.UserToRoles.Update(userToRole);
                else
                    await context.UserToRoles.AddAsync(userToRole);
                await context.SaveChangesAsync();
            }
            return host;
        }
        public async static Task<IHost> ApplyRoleSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            foreach (var role in RoleSeeds.Roles)
            {
                if (await context.Roles.AnyAsync(c => c.Id == role.Id))
                    context.Roles.Update(role);
                else
                    await context.Roles.AddAsync(role);
                await context.SaveChangesAsync();
            }

            return host;
        }

        public async static Task<IHost> ApplyReviewSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            foreach (var review in ReviewSeeds.GetReviews())
            {
                if (await context.Reviews.AnyAsync(r => r.Id == review.Id))
                {
                    context.Reviews.Update(review);
                }
                else
                {
                    await context.Reviews.AddAsync(review);
                }

                await context.SaveChangesAsync();
            }

            return host;
        }

        public async static Task<IHost> ApplyToDoTaskSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            foreach (var task in ToDoTasksSeeds.Tasks)
            {
                if (await context.ToDoTask.AnyAsync(c => c.Id == task.Id))
                    context.ToDoTask.Update(task);
                else
                    await context.ToDoTask.AddAsync(task);
                await context.SaveChangesAsync();
            }

            return host;
        }

        public async static Task<IHost> ApplyUserToTaskSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            foreach (var userToTask in UserToTaskSeeds.UserToTasks)
            {
                if (await context.UserToTask.AnyAsync(c => c.UserId == userToTask.UserId && c.ToDoTaskId == userToTask.ToDoTaskId))
                    context.UserToTask.Update(userToTask);
                else
                    await context.UserToTask.AddAsync(userToTask);
                await context.SaveChangesAsync();
            }
            return host;

        }




        public static async Task<IHost> ApplyInterviewSeeding(this IHost host){
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            
            foreach(var interview in InterviewSeeds.GetInterviews())
            {
                if(await context.Interviews.AnyAsync(i => i.Id == interview.Id))
                {
                    context.Interviews.Update(interview);
                }
                else
                {
                    await context.Interviews.AddAsync(interview);
                }
                await context.SaveChangesAsync();
            }
            
            return host;
        }
        public static async Task<IHost> ApplyUsersToInterviews(this IHost host){
            using var scope = host.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            context.UsersToInterviews.RemoveRange(context.UsersToInterviews.Where(t => InterviewSeeds.interviewIds.Contains(t.InterviewId)));
            
            foreach(var userToInterview in UsersToInterviewSeeds.GetUsersTosInterviews())
            {
                if(await context.UsersToInterviews.AnyAsync(i => i.Id == userToInterview.Id))
                {
                    context.UsersToInterviews.Update(userToInterview);
                }
                else
                {
                    await context.UsersToInterviews.AddAsync(userToInterview);
                }
                await context.SaveChangesAsync();
            }
            

            return host;
        }
    }
}
