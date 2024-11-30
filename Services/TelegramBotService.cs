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

                 //   var priceChange7d = await _pricesApiProvider.GetPricesChange7d(PriceApiServiceKeys.Coingecko, cryptoSymbols);

                    var news = await GetNewsAsync(cryptoSymbols, weekAgo, now);

                    //var analysis = await _analysisService.AnalyzeAsync(cryptoSymbols, priceChange7d, news);

                    //await _botClient.SendTextMessageAsync(update.Message.Chat.Id, analysis);
                }
            }
        }

        private Task HandleErrorAsync(Exception exception)
        {
            Console.WriteLine($"Error: {exception.Message}");
            return Task.CompletedTask;
        }

        private async Task<List<NewsArticle>> GetNewsAsync(string keyword, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {

            List<NewsArticle> allNews = new();


            var sd = new List<NewsApiServiceKeys>((NewsApiServiceKeys[])Enum.GetValues(typeof(NewsApiServiceKeys)));

            var searchTasks = sd
                .Where(x => x == NewsApiServiceKeys.NewsapiOrg)
    .Select(x => Task.Run(() =>
            GetApiNewsAsync(allNews, x, keyword, startDate, endDate, cancellationToken),
        cancellationToken))
    .ToList();
            await Task.WhenAll(searchTasks);

            return allNews;
        }

        private async Task GetApiNewsAsync(List<NewsArticle> allNews, NewsApiServiceKeys serviceKey, string keyword, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            var news = await   _newsApiProvider.GetNewsAsync(serviceKey, keyword, startDate, endDate, cancellationToken);
            allNews.AddRange(news);
        }
    }
}
