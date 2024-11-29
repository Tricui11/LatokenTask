using LatokenTask.Models;

namespace LatokenTask.Services.Abstract
{
    public interface IAnalysisService
    {
        Task<string> AnalyzeAsync(string cryptoSymbol, decimal priceChange, List<NewsArticle> newsArticles);
    }
}
