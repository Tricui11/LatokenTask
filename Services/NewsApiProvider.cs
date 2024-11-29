using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace LatokenTask.Services;

public class NewsApiProvider : INewsApiProvider
{
    private readonly IServiceProvider _serviceProvider;
   // private readonly ILogger<SupplierApiProvider> _logger;

    public NewsApiProvider(IServiceProvider serviceProvider)
       // ILogger<SupplierApiProvider> logger)
    {
        _serviceProvider = serviceProvider;
       // _logger = logger;
    }


    public async Task<List<NewsArticle>> GetNewsAsync(NewsApiServiceKeys serviceKey, string keyword, DateTime startDate, DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var apiService = _serviceProvider.GetKeyedService<INewsService>(serviceKey);

            var news = await apiService.GetNewsAsync(keyword, startDate, endDate);

            return news;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Ошибка при получении данных от поставщика: {SupplierService}",
            //    apiService.GetType().Name);

            return new List<NewsArticle>();
        }
    }
}