using AiGettingStartedSemanticKernel;
using Json.More;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;

// ---1. Creating the Kernel---
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

var kernel = builder.Build();
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();


// ---2. Simple prompt invocation---
/*var result = await kernel.InvokePromptAsync("Give me a list of weekly household tasks");
Console.WriteLine(result.ToString());


// ---3. Prompt templates---
var prompt = "What are some fun things to do if I'm visiting {{$city}}?";

var activitiesFunction = kernel.CreateFunctionFromPrompt(prompt);
var arguments = new KernelArguments { ["city"] = "San Antonio" };

// Either of these ways works and gives the same result:
// a. Call InvokeAsync() on the function object
var result2 = await activitiesFunction.InvokeAsync(kernel, arguments);
Console.WriteLine(result2);

// b. Call InvokeAsync() on the kernel object
result2 = await kernel.InvokeAsync(activitiesFunction, arguments);
Console.WriteLine(result2);


// ---4. Handlebars prompt templates---
var handleBarsPrompt =  """
                            <message role="system">Instructions: Identify the from and to destinations
                            and dates from the user's request</message>

                            <message role="user">Can you give me a list of flights from Seattle to Tokyo?
                            I want to travel from March 11 to March 18.</message>

                            <message role="assistant">
                            Origin: Seattle
                            Destination: Tokyo
                            Depart: 03/11/2025
                            Return: 03/18/2025
                            </message>

                            <message role="user">{{input}}</message>
                        """;

var handleBarsInput = "I want to travel from May 18 to July 24. I want to go to Norway for NDC Oslo. I live in San Antonio.";
var handleBarsArguments = new KernelArguments { ["input"] = handleBarsInput };

var config = new PromptTemplateConfig
{
    Template = handleBarsPrompt,
    TemplateFormat = "handlebars",
    Name = "FlightPrompt"
};
var handleBarsFunction = kernel.CreateFunctionFromPrompt(config, new HandlebarsPromptTemplateFactory());
var result3 = await kernel.InvokeAsync(handleBarsFunction, handleBarsArguments);
Console.WriteLine(result3);


// ---5. Chat history---
var settings = new OpenAIPromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

var history = new ChatHistory();
history.AddSystemMessage("You are an assistant, but respond kind of rudely and annoyed." +
                         "Apologize after, though, because it's been kind of a rough week for you.");

while (true)
{
    var userInput = Console.ReadLine();
    if (string.IsNullOrEmpty(userInput)) break;

    history.AddUserMessage(userInput);

    var result4 = await chatCompletionService.GetChatMessageContentsAsync(
        history,
        settings,
        kernel
    );

    foreach (var chunk in result4)
    {
        history.AddAssistantMessage(chunk.ToString());
        Console.WriteLine(chunk);
    }
}*/


// ---6. Using a plugin---
kernel.Plugins.AddFromType<TaskManagementPlugin>("TaskManagement");
var pluginArguments = new KernelArguments { ["id"] = 1 };
var result5 = await kernel.InvokeAsync("TaskManagement", "complete_task", pluginArguments);
Console.WriteLine(result5);

// Note: you can view the generated JSON schemas for the plugin functions this way
foreach (var tool in kernel.Plugins["TaskManagement"])
{
    Console.WriteLine(tool.JsonSchema.ToString());
}


// ---7. Using a plugin with chat history---
ChatHistory history2 = [];
history2.AddUserMessage("What are all of the critical tasks?");

var settings2 = new OpenAIPromptExecutionSettings
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

var result6 = await chatCompletionService.GetChatMessageContentsAsync(
    history2,
    settings2,
    kernel
);
foreach (var chunk in result6)
{
    history2.AddAssistantMessage(chunk.ToString());
    Console.WriteLine(chunk);
}

while (true)
{
    var userInput2 = Console.ReadLine();
    if (string.IsNullOrEmpty(userInput2)) break;

    history2.AddUserMessage(userInput2);

    var result7 = await chatCompletionService.GetChatMessageContentsAsync(
        history2,
        settings2,
        kernel
    );

    foreach (var chunk in result6)
    {
        history2.AddAssistantMessage(chunk.ToString());
        Console.WriteLine(chunk);
    }
}