using LatokenTask.Services;
using LatokenTask.Services.Abstract;
using LatokenTask.ExternalApis.NewsapiOrg;
using LatokenTask.ExternalApis.GnewsIo;
using LatokenTask.ExternalApis.Cryptopanic;
using LatokenTask.ExternalApis.Coinmarketcap;
using LatokenTask.ExternalApis.Coingecko;
using LatokenTask.Mapping;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = Host.CreateDefaultBuilder(args);

ConfigureLogging(builder);

builder.ConfigureServices((context, services) =>
{
    var config = context.Configuration;
    string openAiApiKey = config["OpenAiApiKey"];
    string telegramBotToken = config["TelegramBotToken"];

    services.AddSemanticKernelServices(openAiApiKey);
    services.AddSingleton<IAnalysisService, AnalysisService>();
    services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(telegramBotToken));
    services.AddSingleton<TelegramBotService>();

    services
    .AddScoped<INewsApiProvider, NewsApiProvider>()
    .AddGnewsIoApiSupport()
    .AddCryptopanicApiSupport()
    .AddNewsapiOrgApiSupport();

    services
    .AddScoped<IPricesApiProvider, PricesApiProvider>()
    .AddCoingeckoApiSupport()
    .AddCoinmarketcapApiSupport();

    services
    .AddCustomMapster();
});

var app = builder.Build();

var botService = app.Services.GetRequiredService<TelegramBotService>();
await botService.StartAsync();

Console.ReadLine();

void ConfigureLogging(IHostBuilder builder)
{
    var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "logs");
    if (!Directory.Exists(logDirectory))
    {
        Directory.CreateDirectory(logDirectory);
    }

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.File(Path.Combine(logDirectory, "log.txt"), rollingInterval: RollingInterval.Day)
        .CreateLogger();

    builder.ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddSerilog();
    });

    var loggerFactory = LoggerFactory.Create(logger =>
    {
        logger.AddSerilog();
    });

    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogInformation("Приложение запущено");

    AppDomain.CurrentDomain.ProcessExit += (sender, e) => Log.CloseAndFlush();
}