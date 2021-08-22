using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace WebAPI.Extensions
{
    public static class CorsExtension
    {
        const string DevelopmentPolicy = "Development_policy_for_angular";
        const string ProductionPolicy = "Production_policy_for_angular";

        public static IServiceCollection AddSpecificCors(this IServiceCollection services)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            switch (environment)
            {
                case "Development":
                    services.AddDevelopmentCorsPolicies();
                    break;
                case "Production":
                    services.AddProductionCorsPolicies();
                    break;
                default:
                    throw new Exception("Unsupported Asp.Net Core environment found. You should add cors policies for that one.");
            }

            return services;
        }

        public static IApplicationBuilder UserSpecificCors(this IApplicationBuilder app)
        {
            app.UseCors(DevelopmentPolicy);
            app.UseCors(ProductionPolicy);

            return app;
        }

        private static IServiceCollection AddDevelopmentCorsPolicies(this IServiceCollection services)
        {
            var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");

            services.AddCors(options =>
            {
                options.AddPolicy(name: DevelopmentPolicy,
                                  builder =>
                                  {
                                      builder
                                        .WithHeaders("Authorization")
                                        .WithHeaders("Content-Type")
                                        .WithMethods("GET", "POST", "PUT", "DELETE")
                                        .WithExposedHeaders("Token-Expired")
                                        .WithOrigins(frontendUrl);
                                  });
            });

            return services;
        }

        private static IServiceCollection AddProductionCorsPolicies(this IServiceCollection services)
        {
            var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");

            services.AddCors(options =>
            {
                options.AddPolicy(name: ProductionPolicy,
                                  builder =>
                                  {
                                      // If you want to test prod environmanet locally 
                                      // relpace next line with this: builder.WithOrigins("http://localhost");
                                      builder
                                        .WithHeaders("Authorization")
                                        .WithHeaders("Content-Type")
                                        .WithMethods("GET", "POST", "PUT", "DELETE")
                                        .WithExposedHeaders("Token-Expired")
                                        .WithOrigins(frontendUrl);
                                  });
            });

            return services;
        }
    }
}
