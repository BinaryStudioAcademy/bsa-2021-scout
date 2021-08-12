using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces.Abstractions;
using Infrastructure.EF;
using Infrastructure.EF.Seeds;
using Infrastructure.Elastic.Seeding;
using Infrastructure.Repositories.Read;
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
        public async static Task<IHost> ApplyElasticSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var client = scope.ServiceProvider.GetService<IElasticClient>();

            await client.IndexManyAsync<ApplicantToTags>(
                ApplicantToTagsSeeds.GetSeed()
            );
            
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
            var otherRepo2 = scope.ServiceProvider.GetService<IReadRepository<Project>>();
            var repo3 = scope.ServiceProvider.GetService<IWriteRepository<User>>();
            var otherRepo3 = scope.ServiceProvider.GetService<IReadRepository<User>>();
            foreach(var id in (await otherRepo.GetEnumerableAsync()).Select(v=>v.Id)){
                await repo.DeleteAsync(id);
            }
            foreach(var id in (await otherRepo2.GetEnumerableAsync()).Select(v=>v.Id)){
                await repo2.DeleteAsync(id);
            }
            foreach(var id in (await otherRepo3.GetEnumerableAsync()).Select(v=>v.Id)){
                await repo3.DeleteAsync(id);
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
