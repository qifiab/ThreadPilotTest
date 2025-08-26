using IF.ThreadPilot.Insurance.API.Features.GetInsurance;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IF.ThreadPilot.Insurance.API.Features
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetInsurance([FromQuery] GetInsuranceQuery command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
