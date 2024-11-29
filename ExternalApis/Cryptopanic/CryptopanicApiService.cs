using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using Microsoft.Extensions.Logging;

namespace LatokenTask.Services;

public class CryptopanicApiService : INewsService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CryptopanicApiService> _logger;

    public CryptopanicApiService(IHttpClientFactory httpClientFactory, ILogger<CryptopanicApiService> logger)
    {
        _logger = logger;
        //_httpClient = httpClientFactory.CreateClient(HttpClientNames.Cryptopanic);
    }

    public Task<List<NewsArticle>> GetNewsAsync(string keyword, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    //public async Task<List<SupplierNewDetailDto>> GetDetailsAsync(string manufacturerName, string article,
    //    bool searchAnalogues,
    //    CancellationToken cancellationToken = default)
    //{
    //    try
    //    {
    //        var response = await _httpClient
    //            .GetAsync($"Search/Products/VendorCode?vendorCode={article}&startsWith=false",
    //                cancellationToken);
    //        if (response.StatusCode == HttpStatusCode.NotFound)
    //        {
    //            return new List<SupplierNewDetailDto>();
    //        }

    //        response.EnsureSuccessStatusCode();
    //        var details = await response.Content.ReadFromJsonAsync<List<LAutoVendorDetail>>(cancellationToken: cancellationToken);

    //        if (searchAnalogues)
    //        {
    //            var getAnaloguesTasks = details.Select(x =>
    //                Task.Run(async () => await GetAnalogues(x.VendorName, x.VendorCode, cancellationToken),
    //                    cancellationToken)).ToList();
    //            await Task.WhenAll(getAnaloguesTasks);
    //            var analoguesDetails = getAnaloguesTasks.SelectMany(x => x.Result).ToList();

    //            details.AddRange(analoguesDetails);
    //        }

    //        var prices = await GetOffers(details.ToList(), cancellationToken);
    //        return prices;
    //    }
    //    catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
    //    {
    //        // ignore
    //        return new List<SupplierNewDetailDto>();
    //    }
    //    catch (Exception e)
    //    {
    //        _logger.LogError(e, "LAuto: Get details request error. Article - {Article}", article);
    //        return new List<SupplierNewDetailDto>();
    //    }
    //}
}