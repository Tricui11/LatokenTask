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

        public async Task<string> AnalyzeAsync(string cryptoSymbol, decimal priceChange, List<NewsArticle> newsArticles)
        {
            var chatMessages = new ChatHistory();
            chatMessages.AddUserMessage($"Analyze the following news to explain why {cryptoSymbol} changed by {priceChange:F2}%:");
            
            string aggregatedNews = string.Join("\n", newsArticles.Select(n => $"{n.Title}: {n.Content}"));
            chatMessages.AddUserMessage(aggregatedNews);
            
            var response = await _kernelService.GetResponseFromChatAsync(chatMessages);
            
            return response;
        }
    }

}
