using System.Net;
using System.Security.Authentication;
using System.Text.Json;

namespace IF.ThreadPilot.Core.Infrastructure.Clients.Http
{
    public abstract class BaseHttpClient
    {
        public enum ApiClient
        {
            Vehicles
        }

        internal readonly HttpClient _client;

        protected BaseHttpClient(IHttpClientFactory httpClientFactory, ApiClient apiClient)
            : this(httpClientFactory.CreateClient(apiClient.ToString()))
        {
        }

        protected BaseHttpClient(HttpClient httpClient)
        {
            _client = httpClient;
        }

        protected async Task<T> GetAsync<T>(string route)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, GetEndPointUrl(route));
            return await SendRequestAsync<T>(request);
        }

        private async Task<T> SendRequestAsync<T>(HttpRequestMessage request)
        {
            using var response = await _client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                var errorMsg = $"a request made to {request.RequestUri} returned: {(int)response.StatusCode} {response.StatusCode}";
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new AuthenticationException(errorMsg);
                }

                throw new HttpRequestException(errorMsg);
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            };

            await using Stream jsonResponse = await response.Content.ReadAsStreamAsync();
            T responseModel = await JsonSerializer.DeserializeAsync<T>(jsonResponse, options);

            return responseModel;
        }

        protected Uri GetEndPointUrl(string route)
        {
            return new Uri($"{_client.BaseAddress?.ToString().TrimEnd('/')}/{route}");
        }
    }
}
