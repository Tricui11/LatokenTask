using LatokenTask.Models;
using LatokenTask.Services.Abstract;

namespace LatokenTask.Services
{
    public class NewsService : INewsService
    {
        private readonly HttpClient _httpClient;

        public NewsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<NewsArticle>> GetNewsAsync(string keyword, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            //string url = $"https://newsapi.org/v2/everything?q={keyword}&from={startDate:yyyy-MM-dd}&to={endDate:yyyy-MM-dd}&apiKey=YOUR_API_KEY";
            //var response = await _httpClient.GetStringAsync(url);
            //var newsData = JsonConvert.DeserializeObject<NewsApiResponse>(response);

            //return newsData.Articles.Select(a => new NewsArticle
            //{
            //    Title = a.Title,
            //    Content = a.Content,
            //    PublishedAt = a.PublishedAt
            //}).ToList();
            // Тестовые данные (например, для Bitcoin)
            var newsArticles = new List<NewsArticle>
            {
                new NewsArticle
                {
                    Title = "Bitcoin hits new high",
                    Content = "Bitcoin reaches a new all-time high, pushing past $50,000.",
                    Source = "Crypto News Daily",
                    PublishedDate = startDate.AddDays(2)
                },
                new NewsArticle
                {
                    Title = "Bitcoin drops after regulatory news",
                    Content = "Bitcoin experiences a drop in price after news about regulatory changes in the US.",
                    Source = "Global Crypto News",
                    PublishedDate = startDate.AddDays(5)
                }
            };

            var filteredNews = newsArticles.Where(article => article.PublishedDate >= startDate && article.PublishedDate <= endDate).ToList();

            return await Task.FromResult(filteredNews);
        }
    }

}
