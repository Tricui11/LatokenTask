using Microsoft.SemanticKernel.ChatCompletion;
using LatokenTask.Models;
using LatokenTask.Services.Abstract;

namespace LatokenTask.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly IKernelService _kernelService;

        public AnalysisService(IKernelService kernelService)
        {
            _kernelService = kernelService;
        }

        public async Task<string> AnalyzeAsync(string cryptoSymbol, List<CryptoPriceInfo> prices, List<NewsArticle> newsArticles)
        {
            var chatMessages = new ChatHistory();

            string priceList = string.Join("\n", prices.Select(p =>
                $"Криптовалюта: {p.Symbol}, Полное название: {p.Name}, Цена: {p.Price:C2}, Изменение за 7 дней: {p.PercentChange7d:F2}%"));

            chatMessages.AddUserMessage($"Символ: {cryptoSymbol}\nДоступные варианты:\n{priceList}");

            string aggregatedNews = string.Join("\n", newsArticles.Select(n => $"{n.Title}: {n.Content}"));
            chatMessages.AddUserMessage($"Анализируйте следующие новости, чтобы объяснить, почему изменения {cryptoSymbol}:");

            chatMessages.AddUserMessage(aggregatedNews);

            chatMessages.AddUserMessage($"Насколько изменилась цена {cryptoSymbol} за 7 дней? " +
                $"Объясни почему цена снизилась или увеличилась на основании предоставленных цен и новостей.");

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
