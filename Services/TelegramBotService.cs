using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace LatokenTask.Services
{
    public class TelegramBotService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IPricesApiProvider _pricesApiProvider;
        private readonly INewsApiProvider _newsApiProvider;
        private readonly IAnalysisService _analysisService;
        private readonly ILogger<TelegramBotService> _logger;

        public TelegramBotService(ITelegramBotClient botClient, IPricesApiProvider pricesApiProvider,
            INewsApiProvider newsApiProvider, IAnalysisService analysisService, ILogger<TelegramBotService> logger)
        {
            _botClient = botClient;
            _pricesApiProvider = pricesApiProvider;
            _newsApiProvider = newsApiProvider;
            _analysisService = analysisService;
            _logger = logger;
        }

        public async Task StartAsync()
        {
            try
            {
                _logger.LogInformation("Starting Telegram bot...");
                _botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while starting the Telegram bot.");
            }
        }

        private async Task HandleUpdateAsync(Update update)
        {
            try
            {
                _logger.LogInformation("Received update: {UpdateId}", update.Id);
                
                if (update.Message is { Text: { } text })
                {
                    string cryptoSymbols = text.ToLower();
                    if (!string.IsNullOrEmpty(cryptoSymbols))
                    {
                        DateTime now = DateTime.UtcNow;
                        DateTime weekAgo = now.AddDays(-7);
                        
                        _logger.LogInformation("Fetching data for symbols: {Symbols} from {StartDate} to {EndDate}",
                            cryptoSymbols, weekAgo, now);
                        
                        var data = await GetApisDataAsync(cryptoSymbols, weekAgo, now);
                        
                        _logger.LogInformation("Data fetched successfully. Analyzing...");
                        
                        var analysis = await _analysisService.AnalyzeAsync(cryptoSymbols, data.prices, data.news);
                        
                        _logger.LogInformation("Analysis complete. Sending response...");
                        
                        await _botClient.SendTextMessageAsync(update.Message.Chat.Id, analysis);
                        
                        _logger.LogInformation("Message sent to chat: {ChatId}", update.Message.Chat.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while handling update: {UpdateId}", update.Id);
            }
        }

        private async Task<(List<NewsArticle> news, List<CryptoPriceInfo> prices)> GetApisDataAsync(string keyword,
            DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            List<NewsArticle> allNews = new();
            List<CryptoPriceInfo> allPrices = new();
            
            try
            {
                _logger.LogInformation("Fetching news and price data from APIs...");
                
                var newsApiServiceKeys = new List<NewsApiServiceKeys>((NewsApiServiceKeys[])Enum.GetValues(typeof(NewsApiServiceKeys)));
                var pricesApiServiceKeys = new List<PriceApiServiceKeys>((PriceApiServiceKeys[])Enum.GetValues(typeof(PriceApiServiceKeys)));
                var dataTasks = newsApiServiceKeys
                    .Select(x => Task.Run(() =>
                    GetApisNewsAsync(allNews, x, keyword, startDate, endDate, cancellationToken),
                    cancellationToken))
                    .Union(pricesApiServiceKeys
                    .Select(x => Task.Run(() =>
                    GetApisPricesAsync(allPrices, x, keyword, cancellationToken),
                    cancellationToken)))
                    .ToList();
                
                await Task.WhenAll(dataTasks);
                
                _logger.LogInformation("All API data fetched.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching data from APIs.");
            }

            return (allNews, allPrices);
        }

        private async Task GetApisNewsAsync(List<NewsArticle> allNews,
            NewsApiServiceKeys serviceKey, string keyword, DateTime startDate, DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching news for service {ServiceKey}...", serviceKey);
                var news = await _newsApiProvider.GetNewsAsync(serviceKey, keyword, startDate, endDate, cancellationToken);
                allNews.AddRange(news);
                _logger.LogInformation("Fetched {NewsCount} news articles.", news.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching news for service {ServiceKey}.", serviceKey);
            }
        }

        private async Task GetApisPricesAsync(List<CryptoPriceInfo> allPrices,
            PriceApiServiceKeys serviceKey, string keyword, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Fetching prices for service {ServiceKey}...", serviceKey);
                var prices = await _pricesApiProvider.GetPricesChange7d(serviceKey, keyword);
                allPrices.AddRange(prices);
                _logger.LogInformation("Fetched {PriceCount} price records.", prices.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching prices for service {ServiceKey}.", serviceKey);
            }
        }

        private Task HandleErrorAsync(Exception exception)
        {
            _logger.LogError(exception, "An error occurred while processing the update.");
            return Task.CompletedTask;
        }
    }
}
