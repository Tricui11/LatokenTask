using LatokenTask.Models;

namespace LatokenTask.Services.Abstract
{
    public interface INewsApiProvider
    {
        Task<List<NewsArticle>> GetNewsAsync(NewsApiServiceKeys serviceKey, string keyword,
            DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}
