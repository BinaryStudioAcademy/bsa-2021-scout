using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using WebAPI.Extensions;

namespace WebAPI
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            (
                await 
                (     
                    await 
                    (
                        await 
                        (
                            (
                                await
                                (
                                    await 
                                    (
                                       
                                        await CreateHostBuilder(args)
                                        .Build()
                                        .ApplyDatabaseMigrations()
                                        .ApplyMongoSeeding()
                                    .ApplyCompanySeeding()
                                ).ApplyElasticSeeding()
                                .ApplyDatabaseSeeding()
                                )
                                .ApplyProjectSeeding()
                            )
                            .ApplyUserSeeding()
                        )
                    ).ApplyVacancySeeding()
                ).ApplyStageSeeding()
            )
           .Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
        }
    }
}
