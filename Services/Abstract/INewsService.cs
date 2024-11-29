using LatokenTask.Models;

namespace LatokenTask.Services.Abstract
{
    public interface INewsService
    {
        Task<List<NewsArticle>> GetNewsAsync(string keyword, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}
