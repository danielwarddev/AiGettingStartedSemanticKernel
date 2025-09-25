using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AiGettingStartedSemanticKernel.McpServer;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConsole(consoleLogOptions =>
{
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<SimpsonsApiTools>();

builder.Services.AddSingleton<HttpClient>(serviceProvider =>
{
    var client = new HttpClient();
    client.BaseAddress = new Uri("https://api.sampleapis.com/simpsons/");
    return client;
});

var host = builder.Build();
await host.RunAsync();