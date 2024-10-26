using CleanArchitectureTemplate.Grpc.Services;

using CleanArchitectureTemplate.Grpc;
using CleanArchitectureTemplate.Grpc.Services;
using CleanArchitectureTemplate.Infrastructure;
using CleanArchitectureTemplate.Application;
using CleanArchitectureTemplate.Domain;
using CleanArchitectureTemplate.Infrastructure.Common;
using CleanArchitectureTemplate.Grpc.Extentions;



var builder = WebApplication.CreateBuilder(args);


// Determine the environment
var isDev = builder.Environment.IsDevelopment();
string? infrastructureDirectory;
if (isDev)
{
    infrastructureDirectory = InfrastructureHelper.GetInfrastructureDirectory();
}
else
{
    infrastructureDirectory = Directory.GetCurrentDirectory();
}

// create configuration object for configuration files of Infrastructure layer
var infrastructureConfiguration = builder.Configuration
    .SetBasePath(infrastructureDirectory!)
    .AddJsonFile("Infrastructure_settings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("serilog.config.json", optional: false, reloadOnChange: true)
    .Build();

builder.Services.AddDomainServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(infrastructureConfiguration);
builder.Services.AddGrpcServices();

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<UserServiceImp>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
