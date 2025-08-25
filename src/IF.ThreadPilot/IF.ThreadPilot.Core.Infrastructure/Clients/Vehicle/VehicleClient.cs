using IF.ThreadPilot.Core.Infrastructure.Clients.Http;
using IF.ThreadPilot.Core.Infrastructure.Clients.Vehicle.Response;
using IF.ThreadPilot.Core.Infrastructure.Configuration.Options;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace IF.ThreadPilot.Core.Infrastructure.Clients.Vehicle
{
    public class VehicleClient : BaseHttpClient, IVehicleClient
    {
        public VehicleClient(IOptions<ApiOptions> config, IHttpClientFactory clientFactory)
            : base(clientFactory, ApiClient.Vehicles)
        {
        }
        public async Task<VehicleResponse> Send(string regNo)
        {
            var result = await GetAsync<VehicleResponse>($"api/InsuranceVehicles?regNo={regNo}");
            return result;

        }
    }
}
