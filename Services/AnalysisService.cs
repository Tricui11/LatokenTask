using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using LatokenTask.Models;
using LatokenTask.Services.Abstract;

namespace LatokenTask.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly Kernel _kernel;

        public AnalysisService()
        {
            var builder = Kernel.CreateBuilder();
            var kernel = builder.Build();
            _kernel = kernel;
        }

        public async Task<string> AnalyzeAsync(string cryptoSymbol, decimal priceChange, List<NewsArticle> newsArticles)
        {
            var chatMessages = new ChatHistory();
            chatMessages.AddUserMessage($"Analyze the following news to explain why {cryptoSymbol} changed by {priceChange:F2}%:");

            string aggregatedNews = string.Join("\n", newsArticles.Select(n => $"{n.Title}: {n.Content}"));
            chatMessages.AddUserMessage(aggregatedNews);

            var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();
            var result = chatCompletionService.GetStreamingChatMessageContentsAsync(chatMessages);

            string fullMessage = "";
            await foreach (var content in result)
            {
                fullMessage += content.Content;
            }

            return fullMessage;
        }
    }

}
