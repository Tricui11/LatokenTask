using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace LatokenTask.Services;

public class NewsApiProvider : INewsApiProvider
{
    private readonly IServiceProvider _serviceProvider;

    public NewsApiProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }


    public async Task<List<NewsArticle>> GetNewsAsync(NewsApiServiceKeys serviceKey, string keyword,
        DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            var apiService = _serviceProvider.GetKeyedService<INewsService>(serviceKey);

            var news = await apiService.GetNewsAsync(keyword, startDate, endDate, cancellationToken);

            return news;
        }
        catch
        {
            return new List<NewsArticle>();
        }
    }
}