using IF.ThreadPilot.Vehicle.API.DTO;
using IF.ThreadPilot.Vehicle.API.Features.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IF.ThreadPilot.Vehicle.API.Features
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceVehiclesController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(VehicleResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVehicle([FromQuery] GetVehicleQuery query, CancellationToken cancellationToken)
        {
            var vehicle = await mediator.Send(query, cancellationToken);

            if (vehicle == null)
                return NotFound();

            return Ok(vehicle);
        }
    }
}
