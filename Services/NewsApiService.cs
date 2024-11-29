using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LatokenTask.Services
{
    public class NewsApiService : INewsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public NewsApiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["ApiKeys:NewsApiKey"];  // Получаем ключ из конфигурации
        }

        public async Task<List<NewsArticle>> GetNewsAsync(string keyword, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return null;
            //string url = $"https://newsapi.org/v2/everything?q={keyword}&from={startDate:yyyy-MM-dd}&to={endDate:yyyy-MM-dd}&apiKey={_apiKey}";
            //var response = await _httpClient.GetStringAsync(url);
            //var data = JsonConvert.DeserializeObject<dynamic>(response);

            //var articles = data.articles as IEnumerable<dynamic>;

            //if (articles != null)
            //{
            //    var newsList = articles.Select(a => new NewsArticle
            //    {
            //        Title = a.title,
            //        Content = a.content,
            //        Source = a.source.name,
            //        PublishedDate = a.publishedAt
            //    }).ToList();
            //}

            //return newsList;
        }
    }
}
