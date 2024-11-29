using LatokenTask.ExternalApis.GnewsIo;
using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace LatokenTask.Services;

public class GnewsIoApiService : INewsService
{
    private readonly HttpClient _httpClient;
    private readonly GnewsIoApiOptions _options;
    private readonly ILogger<GnewsIoApiService> _logger;

    public GnewsIoApiService(IHttpClientFactory httpClientFactory,
        GnewsIoApiOptions options,
        ILogger<GnewsIoApiService> logger)
    {
        _logger = logger;
        _options = options;
        _httpClient = httpClientFactory.CreateClient(HttpClientNames.GnewsIo);
    }

    public async Task<List<NewsArticle>> GetNewsAsync(string keyword,
        DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient
                .GetAsync($"search?q={keyword}&lang=en&country=us&apikey={_options.ApiKey}",
                    cancellationToken);

            var sd = await response.Content.ReadAsStringAsync();    

            if (!response.IsSuccessStatusCode)
            {
                return new List<NewsArticle>();
            }

            response.EnsureSuccessStatusCode();
           var data = await response.Content.ReadFromJsonAsync<GnewsIoNewsResponseDto>(cancellationToken: cancellationToken);

            var news = data.Articles.Select(x => new NewsArticle()
            {
                Source = "newsapi.org",
                Title = x.Title,
                Content = x.Content,
                PublishedDate = x.PublishedAt
            }).ToList();
            
            return news;
        }
        catch (Exception e)
        {
          //  _logger.LogError(e, "LAuto: Get details request error. Article - {Article}", article);
            return new List<NewsArticle>();
        }






    }
}