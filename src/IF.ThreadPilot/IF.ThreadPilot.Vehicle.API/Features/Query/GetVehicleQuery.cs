using IF.ThreadPilot.Vehicle.API.DTO;
using MediatR;

namespace IF.ThreadPilot.Vehicle.API.Features.Query
{
    public record GetVehicleQuery(string regNo) : IRequest<VehicleResponseDto?>;
}
