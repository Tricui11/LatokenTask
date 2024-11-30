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
                $"Название: {p.Name}, Цена: {p.Price:C}, Изменение за 7 дней: {p.PercentChange7d}%"));

            chatMessages.AddUserMessage($"Символ: {cryptoSymbol}\nДоступные варианты:\n{priceList}");

            string aggregatedNews = string.Join("\n", newsArticles.Select(n => $"{n.Title}: {n.Content}"));
            chatMessages.AddUserMessage($"Анализируйте следующие новости, чтобы объяснить, почему изменения {cryptoSymbol}:");

            chatMessages.AddUserMessage(aggregatedNews);

            chatMessages.AddUserMessage($"Пожалуйста, учтите только эти новости и данные, " +
                $"чтобы сформировать выводы о текущем изменении {cryptoSymbol}. Старые данные не должны использоваться.");

            var response = await _kernelService.GetResponseFromChatAsync(chatMessages);

            return response;
        }
    }
}
