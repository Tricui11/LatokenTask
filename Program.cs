//using Microsoft.SemanticKernel;
//using Microsoft.SemanticKernel.ChatCompletion;
//using Microsoft.SemanticKernel.Connectors.OpenAI;

//var builder = Kernel.CreateBuilder();

//builder.AddOpenAIChatCompletion(
//      "gpt-4-1106-preview",
//         Environment.GetEnvironmentVariable("openai-api-key"));

//var kernel = builder.Build();

//IChatCompletionService chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

//ChatHistory chatMessages = new ChatHistory();

//while (true)
//{
//    System.Console.Write("User > ");

//    chatMessages.AddUserMessage(Console.ReadLine()!);

//    var result = chatCompletionService.GetStreamingChatMessageContentsAsync(
//        chatMessages,
//        executionSettings: new OpenAIPromptExecutionSettings()
//        {
//            ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
//            Temperature = 0
//        },
//        kernel: kernel);

//    string fullMessage = "";

//    await foreach (var content in result)
//    {
//        if (content.Role.HasValue)
//            System.Console.Write("\nAssistant > ");

//        System.Console.Write(content.Content);
//        fullMessage += content.Content;
//    }

//    System.Console.WriteLine();
//    chatMessages.AddAssistantMessage(fullMessage);
//}
using System.Net.Http;
using LatokenTask.Services;

var botClient = new TelegramBotClient("7518387244:AAE2bNofPh8msUZRuz5zo6y13zN0gYqGR84");
HttpClient httpClient = new HttpClient();
var priceService = new PriceService(httpClient); // Реализация IPriceService
var newsService = new NewsService(httpClient);   // Реализация INewsService
var analysisService = new AnalysisService(); // Реализация IAnalysisService

var botService = new TelegramBotService(botClient, priceService, newsService, analysisService);

await botService.StartAsync();

// Задержка приложения
Console.ReadLine();