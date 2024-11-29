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

        public async Task<List<NewsArticle>> GetNewsAsync(string keyword, DateTime startDate, DateTime endDate)
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
            return null;
        }
    }

}
