using Microsoft.SemanticKernel.ChatCompletion;
using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using System.Globalization;

namespace LatokenTask.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly IKernelService _kernelService;

        public AnalysisService(IKernelService kernelService)
        {
            _kernelService = kernelService;
        }

        public async Task<string> AnalyzeAsync(string cryptoSymbol,
            IEnumerable<CryptoPriceInfo> prices, IEnumerable<NewsArticle> newsArticles)
        {
            var chatMessages = new ChatHistory();

            string priceList = string.Join("\n", prices.Select(p =>
                $"Криптовалюта: {p.Symbol}, Полное название: {p.Name}, Цена: " +
                $"{p.Price.ToString("C2", CultureInfo.GetCultureInfo("en-US"))}, " +
                $"Изменение за 7 дней: {p.PercentChange7d:F2}%. Источник: {p.Source}"));

            chatMessages.AddUserMessage($"Символ: {cryptoSymbol}\nДоступные варианты:\n{priceList}");

            string aggregatedNews = string.Join("\n", newsArticles.Select(n => $"{n.Title}: {n.Content}. Источник {n.Source}"));
            chatMessages.AddUserMessage($"Анализируйте следующие новости, чтобы объяснить, почему изменения {cryptoSymbol}:");

            chatMessages.AddUserMessage(aggregatedNews);

            chatMessages.AddUserMessage($"Насколько изменилась цена {cryptoSymbol} за 7 дней? " +
                $"Объясни почему цена снизилась или увеличилась на основании предоставленных цен и новостей.");

            chatMessages.AddUserMessage($"Пожалуйста, начните ответ с копирования строки {priceList}, которую я Вам здесь предоставил.");

            chatMessages.AddUserMessage($"Важно: Пожалуйста, учтите только эти новости, чтобы объяснить, " +
                $"почему произошли изменения в цене {cryptoSymbol}. " +
                "Не используйте старые данные или источники за пределами предоставленных новостей. " +
                "Вам нужно сформировать выводы только на основе " +
                "текущих новостей и информации о ценах, которые я вам предоставил.");

            var response = await _kernelService.GetResponseFromChatAsync(chatMessages);

            return response;
        }
    }
}
