using LatokenTask.ExternalApis;
using LatokenTask.ExternalApis.Coinmarketcap;
using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace LatokenTask.Services;

public class CoinmarketcapApiService : IPricesService
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly ILogger<CoinmarketcapApiService> _logger;

    public CoinmarketcapApiService(IHttpClientFactory httpClientFactory,
        IMapper mapper,
        ILogger<CoinmarketcapApiService> logger)
    {
        _mapper = mapper;
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient(HttpClientNames.Coinmarketcap);
    }

    public async Task<List<CryptoPriceInfo>> GetPricesChange7d(string cryptoSymbols, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Start fetching prices from coinmarketcap for keyword: {cryptoSymbols}",
                cryptoSymbols);

            var requestUri = $"cryptocurrency/quotes/latest?symbol={cryptoSymbols}";

            var response = await _httpClient.GetAsync(requestUri, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to fetch prices from coinmarketcap. Status Code: {StatusCode}, Request: {RequestUri}",
                    response.StatusCode, requestUri);
                return new List<CryptoPriceInfo>();
            }

            response.EnsureSuccessStatusCode();

            var responseDto = await response.Content.ReadFromJsonAsync<CoinMarketCapResponseDto>(cancellationToken: cancellationToken);

            if (responseDto?.Data == null || responseDto.Data.Count == 0)
            {
                _logger.LogWarning("No prices found for the given query: {Keyword}", cryptoSymbols);
                return new List<CryptoPriceInfo>();
            }

            var res = _mapper.Map<List<CryptoPriceInfo>>(responseDto.Data.SelectMany(x => x.Value));

            _logger.LogInformation("Successfully fetched {PricesCount} prices from coinmarketcap.", res.Count);

            return res;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while fetching prices from coinmarketcap.");
            return new List<CryptoPriceInfo>();
        }
    }
}