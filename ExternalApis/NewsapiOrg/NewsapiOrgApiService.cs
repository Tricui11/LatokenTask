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
            var response = await _httpClient
                .GetAsync($"everything?q={keyword}&from=2024-11-28&to=2024-11-28&sortBy=popularity&apiKey={_options.ApiKey}",
                    cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                return new List<NewsArticle>();
            }



            response.EnsureSuccessStatusCode();
           var data = await response.Content.ReadFromJsonAsync<NewsapiOrgNewsResponseDto>(cancellationToken: cancellationToken);



            var news =  _mapper.Map<List<NewsArticle>>(data.Articles);



            //var news = data.Articles.Select(x => new NewsArticle()
            //{
            //    Source = "newsapi.org",
            //    Title = x.Title,
            //    Content = x.Content,
            //    PublishedDate = x.PublishedAt
            //}).ToList();
            
            return news;
        }
        catch (Exception e)
        {
          //  _logger.LogError(e, "LAuto: Get details request error. Article - {Article}", article);
            return new List<NewsArticle>();
        }






    }
}