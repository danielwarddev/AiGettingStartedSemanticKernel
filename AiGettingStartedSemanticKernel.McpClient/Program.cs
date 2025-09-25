using Microsoft.SemanticKernel;
using ModelContextProtocol.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Connectors.OpenAI;

// Create an MCP client from our local McpServer project
var serverProjectPath = Path.GetFullPath(
    Path.Combine(
        AppDomain.CurrentDomain.BaseDirectory,
        "..", "..", "..", "..",
        "AiGettingStartedSemanticKernel.McpServer"
    )
);
var clientTransport = new StdioClientTransport(new StdioClientTransportOptions
{
    Name = "ExampleMcpClient",
    Command = "dotnet",
    Arguments = ["run", "--project", serverProjectPath],
    StandardErrorLines = Console.WriteLine
});

var mcpClient = await McpClientFactory.CreateAsync(clientTransport);

// List the tools in the MCP client
var simpsonsTools = await mcpClient.ListToolsAsync();
foreach (var tool in simpsonsTools)
{
    Console.WriteLine($"- {tool.Name}: {tool.Description}");
}

// Bonus example - tools provided by the GitHub MCP server
await using IMcpClient githubMcpClient = await McpClientFactory.CreateAsync(new StdioClientTransport(new()
{
    Name = "GitHub",
    Command = "npx",
    Arguments = ["-y", "@modelcontextprotocol/server-github"],
}));

var ghTools = await githubMcpClient.ListToolsAsync();
foreach (var tool in ghTools)
{
    Console.WriteLine($"- {tool.Name}: {tool.Description}");
}

// 1. Example usage: call the get_photo tool manually
var getCharacterTool = simpsonsTools.FirstOrDefault(t => t.Name == "get_simpsons_character");
var characterResult = await mcpClient.CallToolAsync(
    getCharacterTool!.Name,
    new Dictionary<string, object?> { ["characterId"] = 5 }
);

// 2. Real-world usage with the kernel: let the LLM decide when to call tools
var kernel = BuildKernel(simpsonsTools);
var settings = new OpenAIPromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};
while (true)
{
    var userInput = Console.ReadLine();
    var result = await kernel.InvokePromptAsync(userInput!, new KernelArguments(settings));
}

Kernel BuildKernel(IList<McpClientTool> tools)
{
    var builder = Kernel.CreateBuilder();
    
    builder.AddAzureOpenAIChatCompletion(
        deploymentName: "gpt-4o-mini",
        endpoint: "https://ai-danielaihub551296452307.openai.azure.com",
        apiKey: Environment.GetEnvironmentVariable("API_KEY")!
    );

    builder.Services.AddLogging(services => services
        .AddConsole()
        .SetMinimumLevel(LogLevel.Information)
    );
    
    var skKernel = builder.Build();
    
#pragma warning disable SKEXP0001
    skKernel.Plugins.AddFromFunctions("Simpsons_Data", tools.Select(tool => tool.AsKernelFunction()));
#pragma warning restore SKEXP0001
    
    return skKernel;
}