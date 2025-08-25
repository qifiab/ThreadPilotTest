using IF.ThreadPilot.Core.Infrastructure.Configuration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IF.ThreadPilot.Core.Infrastructure.Configuration.Dependencies
{
    public static class AppsettingsOptionsConfigurations
    {
        public static IServiceCollection RegisterConfigOptions(this IServiceCollection services, IConfiguration config)
        {
            return services
                .Configure<ApiOptions>(options => config.GetSection("Api").Bind(options))
                .Configure<VehicleOptions>(options => config.GetSection("Api:Vehicle").Bind(options));
                
        }
    }
}
