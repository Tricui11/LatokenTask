using LatokenTask.Services.Abstract;
using Telegram.Bot.Types;

namespace LatokenTask.Services
{
    public class TelegramBotService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IPriceService _priceService;
        private readonly INewsService _newsService;
        private readonly IAnalysisService _analysisService;

        public TelegramBotService(ITelegramBotClient botClient, IPriceService priceService, INewsService newsService, IAnalysisService analysisService)
        {
            _botClient = botClient;
            _priceService = priceService;
            _newsService = newsService;
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
                string cryptoSymbol = text.ToLower();
                if (!string.IsNullOrEmpty(cryptoSymbol))
                {
                    DateTime now = DateTime.UtcNow;
                    DateTime weekAgo = now.AddDays(-7);

                    var prices = await _priceService.GetPriceHistoryAsync(cryptoSymbol, weekAgo, now);
                    var priceChange = _priceService.CalculatePriceChange(prices);

                    var news = await _newsService.GetNewsAsync(cryptoSymbol, weekAgo, now);

                    var analysis = await _analysisService.AnalyzeAsync(cryptoSymbol, priceChange, news);

                    await _botClient.SendTextMessageAsync(update.Message.Chat.Id, analysis);
                }
            }
        }

        private Task HandleErrorAsync(Exception exception)
        {
            Console.WriteLine($"Error: {exception.Message}");
            return Task.CompletedTask;
        }
    }
}
