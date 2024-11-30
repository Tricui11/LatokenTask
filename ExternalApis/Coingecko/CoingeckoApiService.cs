using LatokenTask.ExternalApis;
using LatokenTask.ExternalApis.Coingecko;
using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http.Json;

namespace LatokenTask.Services;

public class CoingeckoApiService : IPricesService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CoingeckoApiService> _logger;
    private List<CoingeckoCryptoCurrencyInfoDto> _cachedCoinsList;
    private readonly object _cacheLock = new();

    public CoingeckoApiService(IHttpClientFactory httpClientFactory, ILogger<CoingeckoApiService> logger)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient(HttpClientNames.Coingecko);
        _cachedCoinsList = null;
    }

    public async Task<List<CryptoPriceInfo>> GetPricesChange7d(string cryptoSymbols, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Start fetching prices from coingecko for keyword: {Keyword}, from {StartDate} to {EndDate}",
                cryptoSymbols);

            var coins = await GetCoinsList(cancellationToken);

            string[] cryptoSubstrings = cryptoSymbols.Split(',').Select(s => s.Trim()).ToArray();

            List<string> searchCryptoSymbols = new();
            foreach (var coin in coins)
            {
                foreach (var cryptoSubstring in cryptoSubstrings)
                {
                    if (string.Equals(coin.Id, cryptoSubstring, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(coin.Name, cryptoSubstring, StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(coin.Symbol, cryptoSubstring, StringComparison.OrdinalIgnoreCase))
                    {
                        searchCryptoSymbols.Add(coin.Id);
                    }
                }
            }

            List<CryptoPriceInfo> res = new();
            var today = DateTime.Today;
            var sevenDaysAgo = DateTime.Today.AddDays(-7);
            foreach (var searchCryptoSymbol in searchCryptoSymbols.Distinct())
            {
                await Task.Delay(250, cancellationToken);
                var currentPriceData = await GetCoinPriceInfoToDate(searchCryptoSymbol, today);
                if (currentPriceData?.MarketData?.CurrentPrice == null)
                {
                    continue;
                }

                await Task.Delay(250, cancellationToken);
                var sevenDaysAgoPriceData = await GetCoinPriceInfoToDate(searchCryptoSymbol, sevenDaysAgo);
                if (sevenDaysAgoPriceData?.MarketData?.CurrentPrice == null)
                {
                    continue;
                }

                var priceToday = currentPriceData.MarketData.CurrentPrice.USD;
                var price7dAgo = sevenDaysAgoPriceData.MarketData.CurrentPrice.USD;
                decimal percentChange7d = (priceToday - price7dAgo) / price7dAgo * 100;
                res.Add(new CryptoPriceInfo()
                {
                    Name = currentPriceData.Name,
                    Symbol = currentPriceData.Symbol,
                    Price = priceToday,
                    PercentChange7d = percentChange7d,
                    Source = "Coingecko"
                });
            }

            _logger.LogInformation("Successfully fetched {PricesCount} prices from coingecko.", res.Count);

            return res;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while getting price from coingecko.");
            return default;
        }
    }

    private async Task<List<CoingeckoCryptoCurrencyInfoDto>> GetCoinsList(CancellationToken cancellationToken = default)
    {
        if (_cachedCoinsList != null)
        {
            _logger.LogInformation("Using cached coins list.");
            return _cachedCoinsList;
        }

        return await GetCoinsListFromApi(cancellationToken);
    }

    private async Task<List<CoingeckoCryptoCurrencyInfoDto>> GetCoinsListFromApi(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching coins list from Coingecko.");

            var response = await _httpClient.GetAsync("coins/list", cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get coins list from Coingecko. Status Code: {StatusCode}, Request: {RequestUri}",
                    response.StatusCode);
                return new List<CoingeckoCryptoCurrencyInfoDto>();
            }

            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<List<CoingeckoCryptoCurrencyInfoDto>>(cancellationToken: cancellationToken);

            lock (_cacheLock)
            {
                _cachedCoinsList = data ?? new List<CoingeckoCryptoCurrencyInfoDto>();  // Кэшируем ответ
            }

            _logger.LogInformation("Successfully fetched and cached coins list from Coingecko.");

            return _cachedCoinsList;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while fetching coins list from Coingecko.");
            return new List<CoingeckoCryptoCurrencyInfoDto>();
        }
    }

    private async Task<CoingeckoCryptoCurrencyDataDto> GetCoinPriceInfoToDate(string cryptoId, DateTime date,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Start getting price from coingecko for keyword: {Keyword}, to {date}",
                cryptoId, date);

            string formattedDate = date.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

            var requestUri = $"coins/{cryptoId}/history?date={formattedDate}&localization=false";

            var response = await _httpClient.GetAsync(requestUri, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Failed to get price from coingecko. Status Code: {StatusCode}, Request: {RequestUri}",
                    response.StatusCode, requestUri);
                return default;
            }

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var data = JsonConvert.DeserializeObject<CoingeckoCryptoCurrencyDataDto>(responseContent);

            _logger.LogInformation("Successfully have get price from coingecko for keyword: {Keyword}, to {date}",
                cryptoId, date);

            return data;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while getting price from coingecko.");
            return default;
        }
    }
}