using LatokenTask.ExternalApis.Cryptopanic;
using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
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
            var response = await _httpClient
                .GetAsync($"posts/?auth_token={_options.Token}&filter=important&currencies={keyword}&kind=news",
                    cancellationToken);

            var sd = await response.Content.ReadAsStringAsync();    

            if (!response.IsSuccessStatusCode)
            {
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
            
            return news;
        }
        catch (Exception e)
        {
          //  _logger.LogError(e, "LAuto: Get details request error. Article - {Article}", article);
            return new List<NewsArticle>();
        }






    }
}