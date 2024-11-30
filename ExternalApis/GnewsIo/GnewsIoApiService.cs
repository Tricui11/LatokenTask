using LatokenTask.ExternalApis;
using LatokenTask.ExternalApis.GnewsIo;
using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace LatokenTask.Services;

public class GnewsIoApiService : INewsService
{
    private readonly HttpClient _httpClient;
    private readonly GnewsIoApiOptions _options;
    private readonly IMapper _mapper;
    private readonly ILogger<GnewsIoApiService> _logger;

    public GnewsIoApiService(IHttpClientFactory httpClientFactory,
        GnewsIoApiOptions options,
        IMapper mapper,
        ILogger<GnewsIoApiService> logger)
    {
        _logger = logger;
        _options = options;
        _mapper = mapper;
        _httpClient = httpClientFactory.CreateClient(HttpClientNames.GnewsIo);
    }

    public async Task<List<NewsArticle>> GetNewsAsync(string keyword,
        DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Start fetching news from gnews.io for keyword: {Keyword}, from {StartDate} to {EndDate}",
                keyword, startDate, endDate);

            var requestUri = $"search?q={keyword}&lang=en&country=us&apikey={_options.ApiKey}";

            var response = await _httpClient.GetAsync(requestUri, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to fetch news from gnews.io. Status Code: {StatusCode}, Request: {RequestUri}",
                    response.StatusCode, requestUri);
                return new List<NewsArticle>();
            }
            
            response.EnsureSuccessStatusCode();
            
            var data = await response.Content.ReadFromJsonAsync<GnewsIoNewsResponseDto>(cancellationToken: cancellationToken);

            if (data?.Articles == null || data.Articles.Count == 0)
            {
                _logger.LogWarning("No articles found for the given query: {Keyword}, from {StartDate} to {EndDate}",
                    keyword, startDate, endDate);
                return new List<NewsArticle>();
            }

            var news = _mapper.Map<List<NewsArticle>>(data.Articles);

            _logger.LogInformation("Successfully fetched {ArticleCount} articles from gnews.io.", news.Count);

            return news;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while fetching news from gnews.io.");

            return new List<NewsArticle>();
        }
    }
}