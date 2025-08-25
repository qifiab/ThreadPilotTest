using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace IF.ThreadPilot.Core.Infrastructure.Configuration
{
    public static class AppSettingsConfiguration
    {
        public static void BuildConfig(this ConfigurationManager configBuilder, IWebHostEnvironment environment)
        {
            var applicationRootPath = Directory.GetCurrentDirectory();

            try
            {
                configBuilder
                    .SetBasePath(applicationRootPath)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();

                if (environment.IsDevelopment())
                {
                    configBuilder.AddLocalDebugConfig();
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException(e.Message);
            }
        }

        private static IConfigurationBuilder AddLocalDebugConfig(this IConfigurationBuilder configBuilder)
        {
            if (Debugger.IsAttached)
            {
                configBuilder.AddJsonFile("appsettings.development.json", optional: true, reloadOnChange: true);
                configBuilder.AddJsonFile("local.settings.json", optional: true, reloadOnChange: true);
            }

            return configBuilder;
        }

    }
}
