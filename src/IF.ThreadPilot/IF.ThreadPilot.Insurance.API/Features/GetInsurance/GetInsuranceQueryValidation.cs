using FluentValidation;

namespace IF.ThreadPilot.Insurance.API.Features.GetInsurance
{
    public class GetInsuranceQueryValidation : AbstractValidator<GetInsuranceQuery>
    {
        public GetInsuranceQueryValidation()
        {
            RuleFor(x => x.SSNo).NotEmpty().Length(11).Matches("^\\d{2}(0[1-9]|1[0-2])(0[1-9]|[12]\\d|3[01])-\\d{4}$");
        }
    }
}
