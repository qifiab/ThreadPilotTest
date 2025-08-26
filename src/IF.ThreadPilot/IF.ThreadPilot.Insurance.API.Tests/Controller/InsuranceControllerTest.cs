using FakeItEasy;
using FluentAssertions;
using IF.ThreadPilot.Core.Infrastructure.Clients.Vehicle;
using IF.ThreadPilot.Core.Infrastructure.Clients.Vehicle.Response;
using IF.ThreadPilot.Core.Infrastructure.Entities.Persistence.Data;
using IF.ThreadPilot.Insurance.API.DTO;
using IF.ThreadPilot.Insurance.API.Features.GetInsurance;
using IF.ThreadPilot.Test.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace IF.ThreadPilot.Insurance.API.Tests.Controller
{
    [Collection("Serial Execution Collection")]
    public class InsuranceControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {


        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public InsuranceControllerTest(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
        }

        [Theory]
        [InlineData("840903-1337")]
        //[InlineData("ABC123", "TDI123", false)]
        public async Task CallingEndpoint_ShouldReturnExpectedResult(string SSno)
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IThreadPilotDbContext>();



            // Arrange
            dbContext.VCustomerInsurances.RemoveRange(dbContext.VCustomerInsurances);
            dbContext.VCustomerInsurances.Add(new Core.Domain.Entities.VCustomerInsurance { Firstname = "Mikael", Surname = "Flood", InsuranceCost = 20, InsuranceName = "Health insurance", InsuranceTypeId = 2, SsNo = SSno, Identity = "Health-123" });
            dbContext.VCustomerInsurances.Add(new Core.Domain.Entities.VCustomerInsurance { Firstname = "Mikael", Surname = "Flood", InsuranceCost = 10, InsuranceName = "Pet insurance", InsuranceTypeId = 1, SsNo = SSno, Identity = "Pet-123" });
            //dbContext.VCustomerInsurances.Add(new Core.Domain.Entities.VCustomerInsurance { Firstname = "Mikael", Surname = "Flood", InsuranceCost = 20, InsuranceName = "Car insurance", InsuranceTypeId = 3, SsNo = SSno, Identity = "ABC123" });
            await dbContext.SaveChangesAsync();



            // Act
            var response = await _client.GetAsync($"api/Insurance?SSno={SSno}");
            // Assert
            response.IsSuccessStatusCode.Should().BeTrue();
            var result = await response.Content.ReadFromJsonAsync<InsuranceResponseDto>();
            result.Should().NotBeNull();
            result.insurances.Count.Should().Be(2);
            result.totalCost.Should().Be(30);

        }


        [Theory]
        [InlineData("840903-1337")]
        //[InlineData("ABC123", "TDI123", false)]
        public async Task CallinghandlerWithVehicleShouldReturnTrue(string SSno)
        {
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IThreadPilotDbContext>();

            var vehicleService = A.Fake<IVehicleClient>();
            var dto = new VehicleResponse("ABC123", "Fiat", "Punto");


            // Fake the Send method to return the response as a completed Task
            A.CallTo(() => vehicleService.Send(A<string>.Ignored))
                .Returns(Task.FromResult(dto));

            var handler = new GetInsuranceQueryHandler(dbContext, vehicleService);


            // Arrange
            dbContext.VCustomerInsurances.RemoveRange(dbContext.VCustomerInsurances);
            dbContext.VCustomerInsurances.Add(new Core.Domain.Entities.VCustomerInsurance { Firstname = "Mikael", Surname = "Flood", InsuranceCost = 20, InsuranceName = "Car insurance", InsuranceTypeId = 3, SsNo = SSno, Identity = "ABC123" });
            await dbContext.SaveChangesAsync();


            var response = await handler.Handle(new GetInsuranceQuery(SSno), CancellationToken.None);

            response.insurances.Should().HaveCount(1);
            response.insurances[0].vehicle.Should().NotBeNull();
            // Act
            //var response = await _client.GetAsync($"api/Insurance?SSno={SSno}");
            // Assert
            //response.IsSuccessStatusCode.Should().BeTrue();
            //var result = await response.Content.ReadFromJsonAsync<InsuranceResponseDto>();
            //result.Should().NotBeNull();
            //result.insurances.Count.Should().Be(2);
            //result.totalCost.Should().Be(30);

        }
    }
}
