using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VerificationProvider.Services;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

var serviceBus = builder.Configuration["ServiceBus"] ?? throw new InvalidOperationException();
builder.Services.AddSingleton(_ => new ServiceBusClient(serviceBus));

builder.Services.AddSingleton<VerificationService>();

builder.Build().Run();