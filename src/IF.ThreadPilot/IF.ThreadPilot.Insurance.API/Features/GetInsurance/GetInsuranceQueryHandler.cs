using IF.ThreadPilot.Core.Infrastructure.Clients.Vehicle;
using IF.ThreadPilot.Core.Infrastructure.Entities.Persistence.Data;
using IF.ThreadPilot.Insurance.API.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IF.ThreadPilot.Insurance.API.Features.GetInsurance
{
    public class GetInsuranceQueryHandler(IThreadPilotDbContext ctx, IVehicleClient vehicleClient) : IRequestHandler<GetInsuranceQuery, InsuranceResponseDto?>
    {
        public async Task<InsuranceResponseDto?> Handle(GetInsuranceQuery request, CancellationToken cancellationToken)
        {
            var result = await ctx.VCustomerInsurances.Where(o => o.SsNo == request.SSNo).ToListAsync();
            if (result.Count() > 0)
            {
                var carInsurances = result
                 .Where(o => o.InsuranceTypeId == 3)
                 .ToList();

                // Run tasks in parallel
                // Run all vehicle API calls in parallel
                var vehicleTasks = carInsurances
                    .Select(async car =>
                    {
                        var vehicleResult = await vehicleClient.Send(car.Identity); // e.g. use Identity/RegNo
                        return (car.Identity, vehicleResult);
                    });

                var vehicleResults = await Task.WhenAll(vehicleTasks);

                // Map car Identity -> vehicle API result
                var vehicleMap = vehicleResults.ToDictionary(v => v.Identity, v => v.vehicleResult);

                var insuranceDtos = result
                    .Select(o => new InsuranceDto(
                        o.InsuranceName,
                        o.Identity,
                        o.InsuranceCost,
                        o.InsuranceTypeId == 3 && vehicleMap.ContainsKey(o.Identity)
                            ? vehicleMap[o.Identity]   // only cars get a vehicle response
                            : null                     // others get no vehicle info
                    ))
                    .ToList();

                var totalCost = insuranceDtos.Sum(i => i.cost);
                return new InsuranceResponseDto(
                        result[0].Firstname,
                        result[0].Surname,
                        totalCost,
                        insuranceDtos
                        );
            }
            return null;
        }
    }
}
