using FluentValidation.AspNetCore;
using FluentValidation;
using IF.ThreadPilot.Core.Infrastructure.Configuration;
using IF.ThreadPilot.Core.Infrastructure.Configuration.Dependencies;
using Microsoft.FeatureManagement;
using System.Reflection;
using IF.ThreadPilot.Core.Infrastructure.Configuration.HttpClientFactory;
using IF.ThreadPilot.Core.Infrastructure.Clients.Vehicle;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
builder.Configuration.BuildConfig(builder.Environment);
services.AddInfrastructureDependenciesWebHosts(builder.Configuration);
services.RegisterConfigOptions(builder.Configuration);
services.AddHttpClientFactoryWithPolly();
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
services.AddValidatorsFromAssemblyContaining(typeof(Program));
services.AddFluentValidationAutoValidation();
// Add services to the container.
services.AddControllers();
services.AddFeatureManagement();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddScoped<IVehicleClient, VehicleClient>();
var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

// Expose the implicitly defined Program class to the test project
public partial class Program
{ }