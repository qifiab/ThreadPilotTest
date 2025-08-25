using IF.ThreadPilot.Core.Infrastructure.Clients.Vehicle.Response;

namespace IF.ThreadPilot.Insurance.API.DTO
{
    public record InsuranceDto(string type, string name, int? cost, VehicleResponse? vehicle = null);
    
}
