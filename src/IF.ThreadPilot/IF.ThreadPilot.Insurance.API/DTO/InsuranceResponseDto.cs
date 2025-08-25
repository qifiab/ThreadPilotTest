namespace IF.ThreadPilot.Insurance.API.DTO
{
    public record InsuranceResponseDto(string firstname, string surname, long? totalCost, List<InsuranceDto> insurances);
    

}
