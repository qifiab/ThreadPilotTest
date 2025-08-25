using IF.ThreadPilot.Core.Infrastructure.Entities.Persistence.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IF.ThreadPilot.Api.Integration.Tests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public string DbName { get; set; } = "InMemoryDbForTesting";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove existing authentication
                services.RemoveAll<IThreadPilotDbContext>();
                services.AddScoped<IThreadPilotDbContext>(x =>
                {
                    var options = new DbContextOptionsBuilder<ThreadPilotDbContext>()
                        .UseInMemoryDatabase(DbName)
                        .Options;
                    return new ThreadPilotDbContext(options);
                });

                // Add a fake authentication scheme for testing if authentication is present

            });
        }
    }
}
