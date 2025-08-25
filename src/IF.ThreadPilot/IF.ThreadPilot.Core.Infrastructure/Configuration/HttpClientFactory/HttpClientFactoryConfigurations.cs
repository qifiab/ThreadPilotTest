using IF.ThreadPilot.Core.Infrastructure.Clients.Http;
using IF.ThreadPilot.Core.Infrastructure.Configuration.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;

namespace IF.ThreadPilot.Core.Infrastructure.Configuration.HttpClientFactory
{
    public static class HttpClientFactoryConfigurations
    {
        public static IServiceCollection AddHttpClientFactoryWithPolly(this IServiceCollection services)
        {
            var apiOption = services.BuildServiceProvider()
                .GetRequiredService<IOptions<ApiOptions>>()
                .Value;

            services.AddHttpClient(BaseHttpClient.ApiClient.Vehicles.ToString(), client =>
            {
                client.BaseAddress = new Uri(apiOption.Vehicle.BaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "text/plain");
                //client.DefaultRequestHeaders.Add("user-key", apiOption..APIKey);
            }).AddTransientHttpErrorPolicy(RetryPolicy);

            return services;
        }

        private static readonly Func<PolicyBuilder<HttpResponseMessage>, AsyncRetryPolicy<HttpResponseMessage>> RetryPolicy = DefaultRetryPolicy;

        private static AsyncRetryPolicy<HttpResponseMessage> DefaultRetryPolicy(PolicyBuilder<HttpResponseMessage> builder)
        {
            return builder.WaitAndRetryAsync([
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            ]);
        }
    }
}
