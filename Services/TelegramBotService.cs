using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using Telegram.Bot.Types;

namespace LatokenTask.Services
{
    public class TelegramBotService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IPricesApiProvider _pricesApiProvider;
        private readonly INewsApiProvider _newsApiProvider;
        private readonly IAnalysisService _analysisService;

        public TelegramBotService(ITelegramBotClient botClient, IPricesApiProvider pricesApiProvider,
            INewsApiProvider newsApiProvider, IAnalysisService analysisService)
        {
            _botClient = botClient;
            _pricesApiProvider = pricesApiProvider;
            _newsApiProvider = newsApiProvider;
            _analysisService = analysisService;
        }

        public async Task StartAsync()
        {
            _botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync);
        }

        private async Task HandleUpdateAsync(Update update)
        {
            if (update.Message is { Text: { } text })
            {
                string cryptoSymbols = text.ToLower();
                if (!string.IsNullOrEmpty(cryptoSymbols))
                {
                    DateTime now = DateTime.UtcNow;
                    DateTime weekAgo = now.AddDays(-7);
                    var data = await GetApisDataAsync(cryptoSymbols, weekAgo, now);

                    var analysis = await _analysisService.AnalyzeAsync(cryptoSymbols, data.prices, data.news);

                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, analysis);
                }
            }
        }

        private async Task<(List<NewsArticle> news, List<CryptoPriceInfo> prices)> GetApisDataAsync(string keyword,
            DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            List<NewsArticle> allNews = new();
            List<CryptoPriceInfo> allPrices = new();

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

            return (allNews, allPrices);
        }

        private async Task GetApisNewsAsync(List<NewsArticle> allNews,
            NewsApiServiceKeys serviceKey, string keyword, DateTime startDate, DateTime endDate,
            CancellationToken cancellationToken = default)
        {
            var news = await   _newsApiProvider.GetNewsAsync(serviceKey, keyword, startDate, endDate, cancellationToken);
            allNews.AddRange(news);
        }

        private async Task GetApisPricesAsync(List<CryptoPriceInfo> allPrices,
            PriceApiServiceKeys serviceKey, string keyword, CancellationToken cancellationToken = default)
        {
            var prices = await _pricesApiProvider.GetPricesChange7d(serviceKey, keyword);
            allPrices.AddRange(prices);
        }

        private Task HandleErrorAsync(Exception exception)
        {
            Console.WriteLine($"Error: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}
