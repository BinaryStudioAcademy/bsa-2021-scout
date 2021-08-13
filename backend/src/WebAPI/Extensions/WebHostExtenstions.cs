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
            foreach(var vacancy in (new VacancySeeds()).Vacancies()){
                await repo.CreateAsync(vacancy);
            }
            
            return host;
        }
        public async static Task<IHost> CleanUp(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var repo = scope.ServiceProvider.GetService<IWriteRepository<Vacancy>>();
            var otherRepo = scope.ServiceProvider.GetService<IReadRepository<Vacancy>>();
            var repo2 = scope.ServiceProvider.GetService<IWriteRepository<Project>>();
            var repo3 = scope.ServiceProvider.GetService<IWriteRepository<User>>();
            var repo4 = scope.ServiceProvider.GetService<IWriteRepository<User>>();
            foreach(var id in (await otherRepo.GetEnumerableAsync()).Select(v=>v.Id)){
                await repo.DeleteAsync(id);
            }

            foreach(var id in ProjectSeeds.Projects.Select(x=>x.Id)){
                await repo2.DeleteAsync(id);
            }
            foreach(var id in UserSeeds.Users.Select(x=>x.Id)){
                await repo3.DeleteAsync(id);
            }
            foreach(var id in CompanySeeds.Companies.Select(x=>x.Id)){
                await repo3.DeleteAsync(id);
            }
            return host;
        }
        public async static Task<IHost> ApplyCompanySeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var repo = scope.ServiceProvider.GetService<IWriteRepository<Company>>();
            var otherRepo = scope.ServiceProvider.GetService<IReadRepository<Company>>();
            foreach(var company in CompanySeeds.Companies){
                await repo.CreateAsync(company);
            }
            
            return host;
        }
        public async static Task<IHost> ApplyProjectSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var repo = scope.ServiceProvider.GetService<IWriteRepository<Project>>();
            var otherRepo = scope.ServiceProvider.GetService<IReadRepository<Project>>();
            foreach(var project in ProjectSeeds.Projects){
                await repo.CreateAsync(project);
            }
            
            return host;
        }
        public async static Task<IHost> ApplyUserSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var repo = scope.ServiceProvider.GetService<IWriteRepository<User>>();
            var otherRepo = scope.ServiceProvider.GetService<IReadRepository<User>>();
            foreach(var user in UserSeeds.Users){
                await repo.CreateAsync(user);
            }
            
            return host;
        }
    }
}
