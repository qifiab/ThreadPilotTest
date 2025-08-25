using IF.ThreadPilot.Core.Infrastructure.Clients.Vehicle;
using IF.ThreadPilot.Core.Infrastructure.Entities.Persistence.Data;
using IF.ThreadPilot.Vehicle.API.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IF.ThreadPilot.Vehicle.API.Features.Query
{
    public class GetVehicleQueryHandler(IThreadPilotDbContext ctx) : IRequestHandler<GetVehicleQuery, VehicleResponseDto?>
    {
        public async Task<VehicleResponseDto?> Handle(GetVehicleQuery request, CancellationToken cancellationToken)
        {
            var vehicle = await ctx.Vehicles
           .AsNoTracking()
           .FirstOrDefaultAsync(v => v.RegNo == request.regNo, cancellationToken);

            if (vehicle == null)
                return null;




            return new VehicleResponseDto(vehicle.RegNo, vehicle.Brand, vehicle.Model);
            
        }
    }
}
