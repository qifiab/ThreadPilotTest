using FluentValidation.AspNetCore;
using FluentValidation;
using IF.ThreadPilot.Core.Infrastructure.Configuration.Dependencies;
using IF.ThreadPilot.Core.Infrastructure.Configuration;
using Microsoft.FeatureManagement;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
builder.Configuration.BuildConfig(builder.Environment);
services.AddInfrastructureDependenciesWebHosts(builder.Configuration);
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
services.AddValidatorsFromAssemblyContaining(typeof(Program));
services.AddFluentValidationAutoValidation();
// Add services to the container.
services.AddControllers();
services.AddFeatureManagement();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

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

app.Run();
