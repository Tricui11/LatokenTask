using LatokenTask.Models;

namespace LatokenTask.Services.Abstract
{
    public interface IAnalysisService
    {
        Task<string> AnalyzeAsync(string cryptoSymbol,
            IEnumerable<CryptoPriceInfo> prices, IEnumerable<NewsArticle> newsArticles);
    }
}
