using FluentAssertions;
using IF.ThreadPilot.Core.Infrastructure.Entities.Persistence.Data;
using IF.ThreadPilot.Test.Base;
using IF.ThreadPilot.Vehicle.API.DTO;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace IF.ThreadPilot.Vehicle.API.Tests.Controllers
{
    public class VehicleControllerTest
    {
        [Collection("Serial Execution Collection")]
        public class VehicleControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
        {
            private readonly HttpClient _client;
            private readonly CustomWebApplicationFactory<Program> _factory;

            public VehicleControllerTests(CustomWebApplicationFactory<Program> factory)
            {
                _client = factory.CreateClient();
                _factory = factory;
            }

            [Theory]
            [InlineData("ABC123", "ABC123", true)]
            [InlineData("ABC123", "TDI123", false)]
            public async Task CallingEndpoint_ShouldReturnExpectedResult(string regNo, string compareRegNr, bool shouldBeTrue)
            {
                using var scope = _factory.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<IThreadPilotDbContext>();

                // Arrange
                dbContext.Vehicles.RemoveRange(dbContext.Vehicles);
                dbContext.Vehicles.Add(new Core.Domain.Entities.Vehicle { Brand = "Zonda", Model = "Revolucion", Owner = "840903-1337", RegNo = regNo });
                await dbContext.SaveChangesAsync();

                // Act
                var response = await _client.GetAsync($"api/InsuranceVehicles?regNo={regNo}");
                // Assert
                response.IsSuccessStatusCode.Should().BeTrue();
                var result = await response.Content.ReadFromJsonAsync<VehicleResponseDto>();
                result.Should().NotBeNull();
                result.regNo.Should().Be(regNo);
                if (shouldBeTrue)
                    result.regNo.Should().Be(regNo);
                else
                    result.regNo.Should().NotBe(compareRegNr);
            }

            [Theory]
            [InlineData("ABC1232")]
            [InlineData("ABC1")]
            public async Task CallingEndpoint_ShouldReturnValidationError(string regNo)
            {
                using var scope = _factory.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<IThreadPilotDbContext>();

                // Arrange
                dbContext.Vehicles.RemoveRange(dbContext.Vehicles);
                dbContext.Vehicles.Add(new Core.Domain.Entities.Vehicle { Brand = "Zonda", Model = "Revolucion", Owner = "840903-1337", RegNo = regNo });
                await dbContext.SaveChangesAsync();

                // Act
                var response = await _client.GetAsync($"api/InsuranceVehicles?regNo={regNo}");
                // Assert
                response.IsSuccessStatusCode.Should().BeFalse();
                var resultString = await response.Content.ReadAsStringAsync();
                var errorResponse = JsonConvert.DeserializeObject<ValidationErrorResponse>(resultString);
                errorResponse.Should().NotBeNull();
                errorResponse.Errors["regNo"].Should().Contain("'reg No' is not in the correct format.");
            }
        }
    }
}
