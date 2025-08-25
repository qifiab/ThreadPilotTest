using IF.ThreadPilot.Insurance.API.DTO;
using MediatR;

namespace IF.ThreadPilot.Insurance.API.Features.GetInsurance
{
    public sealed record GetInsuranceQuery(string SSNo) : IRequest<InsuranceResponseDto?>;
    
}
