using IF.ThreadPilot.Core.Infrastructure.Clients.Vehicle.Response;

namespace IF.ThreadPilot.Core.Infrastructure.Clients.Vehicle
{
    public interface IVehicleClient
    {
        public Task<VehicleResponse> Send(string regNo);
    }
}
