﻿using LatokenTask.Services;
using LatokenTask.Services.Abstract;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using LatokenTask.ExternalApis;
using LatokenTask.ExternalApis.NewsapiOrg;
using LatokenTask.ExternalApis.GnewsIo;
using LatokenTask.ExternalApis.Cryptopanic;

var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((context, services) =>
{
    var config = context.Configuration;
    string openAiApiKey = config["OpenAiApiKey"];
    string telegramBotToken = config["TelegramBotToken"];

    services.AddSemanticKernelServices(openAiApiKey);
    services.AddHttpClient<IPriceService, PriceService>();
    services.AddHttpClient<INewsService, NewsService>();
    services.AddSingleton<IAnalysisService, AnalysisService>();
    services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(telegramBotToken));
    services.AddSingleton<TelegramBotService>();




    services
    .AddScoped<INewsApiProvider, NewsApiProvider>()
    .AddGnewsIoApiSupport()
    .AddCryptopanicApiSupport()
    .AddNewsapiOrgApiSupport();
    //.AddArmtekApiSupport()
    //.AddMlAutoApiSupport();






});

var app = builder.Build();

var botService = app.Services.GetRequiredService<TelegramBotService>();
await botService.StartAsync();

Console.ReadLine();