using LatokenTask.ExternalApis.Cryptopanic;
using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace LatokenTask.Services;

public class CryptopanicApiService : INewsService
{
    private readonly HttpClient _httpClient;
    private readonly CryptopanicApiOptions _options;
    private readonly ILogger<CryptopanicApiService> _logger;

    public CryptopanicApiService(IHttpClientFactory httpClientFactory,
        CryptopanicApiOptions options,
        ILogger<CryptopanicApiService> logger)
    {
        _logger = logger;
        _options = options;
        _httpClient = httpClientFactory.CreateClient(HttpClientNames.Cryptopanic);
    }

    public async Task<List<NewsArticle>> GetNewsAsync(string keyword,
        DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Start fetching news from cryptopanic for keyword: {Keyword}, from {StartDate} to {EndDate}",
                keyword, startDate, endDate);

            var requestUri = $"posts/?auth_token={_options.Token}&filter=important&currencies={keyword}&kind=news";

            var response = await _httpClient.GetAsync(requestUri, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to fetch news from cryptopanic. Status Code: {StatusCode}, Request: {RequestUri}",
                    response.StatusCode, requestUri);
                return new List<NewsArticle>();
            }

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken: cancellationToken);

            var news = new List<NewsArticle>();
            foreach (var item in jsonResponse.GetProperty("results").EnumerateArray())
            {
                var title = item.GetProperty("title").GetString();
                var publishedAt = item.GetProperty("published_at").GetDateTime();
                var url = item.GetProperty("url").GetString();
                news.Add(new NewsArticle() { Title = title, PublishedAt = publishedAt, Content = url });
            }

            _logger.LogInformation("Successfully fetched {ArticleCount} articles from cryptopanic", news.Count);

            return news;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while fetching news from cryptopanic");

            return new List<NewsArticle>();
        }
    }
}