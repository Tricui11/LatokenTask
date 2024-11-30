using LatokenTask.Models;

namespace LatokenTask.Services.Abstract
{
    public interface IAnalysisService
    {
        Task<string> AnalyzeAsync(string cryptoSymbol, List<CryptoPriceInfo> prices, List<NewsArticle> newsArticles);
    }
}
