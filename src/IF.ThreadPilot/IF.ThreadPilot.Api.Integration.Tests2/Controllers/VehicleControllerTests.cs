using Azure.Core;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using IF.ThreadPilot.Core.Infrastructure.Entities.Persistence.Data;

namespace IF.ThreadPilot.Api.Integration.Tests.Controllers
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
            var result = await response.Content.ReadAsStringAsync();
            result.Should().NotBeNull();

            var json = JsonConvert.DeserializeObject<JObject>(result);
            json.Should().NotBeNull();

            var id = json["value"]?[0]?["RegNr"]?.Value<string>(); // Extract "Id" from the first object in "value" array
            if (shouldBeTrue)
                id.Should().Be(compareRegNr);
            else
                id.Should().NotBe(compareRegNr);
        }
    }
}
