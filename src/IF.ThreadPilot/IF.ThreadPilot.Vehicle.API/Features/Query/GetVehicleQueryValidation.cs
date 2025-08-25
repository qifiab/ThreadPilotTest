using FluentValidation;

namespace IF.ThreadPilot.Vehicle.API.Features.Query
{
    public class GetVehicleQueryValidation : AbstractValidator<GetVehicleQuery>
    {
        public GetVehicleQueryValidation() { 
            RuleFor(o => o.regNo).NotEmpty().Matches("^(?:[A-HJ-PR-UW-Z]{3}[ -]?(?:\\d{3}|\\d{2}[A-HJ-NP-RTUW-Z]))$");
        }
    }
}
