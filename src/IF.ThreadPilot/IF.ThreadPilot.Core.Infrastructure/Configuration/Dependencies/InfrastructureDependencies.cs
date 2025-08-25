using IF.ThreadPilot.Core.Infrastructure.Entities.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace IF.ThreadPilot.Core.Infrastructure.Configuration.Dependencies
{
    public static class InfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureDependenciesWebHosts(this IServiceCollection services, IConfiguration config)
        {
            services.AddBaseInfrastructureDependencies(config);
            return services;
        }
        private static IServiceCollection AddBaseInfrastructureDependencies(this IServiceCollection services, IConfiguration config)
        {
            services.AddApplicationDatabase(config);

            return services;
        }



        private static IServiceCollection AddApplicationDatabase(this IServiceCollection services, IConfiguration config)
        {
            var ConnectionString = config.GetConnectionString("ThreadPilot");
            if (string.IsNullOrWhiteSpace(ConnectionString))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("ConnectionString ThreadPilot is empty");

                Console.ResetColor();
            }

            if (Debugger.IsAttached)
            {
                Console.WriteLine("Using ThreadPilot connectionString => ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(ConnectionString);
                Console.ResetColor();
            }

            services
                .AddDbContext<ThreadPilotDbContext>((serviceProvider, options) =>
                        options.UseNpgsql(ConnectionString, builder =>
                        {
                            builder.EnableRetryOnFailure(maxRetryCount: 5);
                            builder.CommandTimeout(240);
                        }),

                    ServiceLifetime.Scoped
                );
            services.AddScoped<IThreadPilotDbContext, ThreadPilotDbContext>();
            return services;
        }
    }
}
