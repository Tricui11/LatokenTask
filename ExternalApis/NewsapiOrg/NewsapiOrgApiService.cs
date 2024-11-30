using LatokenTask.ExternalApis.NewsapiOrg;
using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace LatokenTask.Services;

public class NewsapiOrgApiService : INewsService
{
    private readonly HttpClient _httpClient;
    private readonly NewsapiOrgApiOptions _options;
    private readonly ILogger<NewsapiOrgApiService> _logger;
    private readonly IMapper _mapper;

    public NewsapiOrgApiService(IHttpClientFactory httpClientFactory,
        NewsapiOrgApiOptions options,
        IMapper mapper,
        ILogger<NewsapiOrgApiService> logger)
    {
        _logger = logger;
        _options = options;
        _mapper = mapper;
        _httpClient = httpClientFactory.CreateClient(HttpClientNames.NewsapiOrg);
    }

    public async Task<List<NewsArticle>> GetNewsAsync(string keyword,
        DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Start fetching news from newsapi.org for keyword: {Keyword}, from {StartDate} to {EndDate}",
                keyword, startDate, endDate);

            string formattedStartDate = startDate.ToString("yyyy-MM-dd");
            string formattedEndDate = endDate.ToString("yyyy-MM-dd");

            var requestUri = $"everything?q={keyword}&from={formattedStartDate}&to={formattedEndDate}&apiKey={_options.ApiKey}";

            var response = await _httpClient.GetAsync(requestUri, cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to fetch news from newsapi.org. Status Code: {StatusCode}, Request: {RequestUri}",
                    response.StatusCode, requestUri);
                return new List<NewsArticle>();
            }
            
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<NewsapiOrgNewsResponseDto>(cancellationToken: cancellationToken);

            if (data?.Articles == null || data.Articles.Count == 0)
            {
                _logger.LogWarning("No articles found for the given query: {Keyword}, from {StartDate} to {EndDate}",
                    keyword, startDate, endDate);
                return new List<NewsArticle>();
            }

            var news =  _mapper.Map<List<NewsArticle>>(data.Articles);

            _logger.LogInformation("Successfully fetched {ArticleCount} articles from newsapi.org.", news.Count);

            return news;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while fetching news from newsapi.org.");
            return new List<NewsArticle>();
        }
    }
}