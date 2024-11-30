using LatokenTask.ExternalApis.Coingecko;
using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Globalization;

namespace LatokenTask.Services;

public class CoingeckoApiService : IPricesService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CoingeckoApiService> _logger;

    public CoingeckoApiService(IHttpClientFactory httpClientFactory,
        ILogger<CoingeckoApiService> logger)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient(HttpClientNames.Coingecko);
    }

    public async Task<List<CryptoPriceInfo>> GetPricesChange7d(string cryptoSymbols, CancellationToken cancellationToken = default)
    {
        try
        {
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
                if (currentPriceData == null)
                {
                    continue;
                }

                await Task.Delay(250, cancellationToken);
                var sevenDaysAgoPriceData = await GetCoinPriceInfoToDate(searchCryptoSymbol, sevenDaysAgo);
                if (sevenDaysAgoPriceData == null)
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

            return res;
        }
        catch (Exception e)
        {
            //  _logger.LogError(e, "LAuto: Get details request error. Article - {Article}", article);
            return default;
        }
    }

    private async Task<List<CoingeckoCryptoCurrencyInfoDto>> GetCoinsList(CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.GetAsync("coins/list", cancellationToken);

            var sd = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            response.EnsureSuccessStatusCode();


            var dtos = JsonConvert.DeserializeObject<List<CoingeckoCryptoCurrencyInfoDto>>(sd);
            return dtos;
        }
        catch (Exception e)
        {
            //  _logger.LogError(e, "LAuto: Get details request error. Article - {Article}", article);
            return default;
        }
    }

    private async Task<CoingeckoCryptoCurrencyDataDto> GetCoinPriceInfoToDate(string cryptoId, DateTime date,
        CancellationToken cancellationToken = default)
    {
        try
        {
        //    cryptoId = "bitcoin-2015-wrapper-meme";
            string formattedDate = date.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);

            var response = await _httpClient
                .GetAsync($"https://api.coingecko.com/api/v3/coins/{cryptoId}/history?date={formattedDate}&localization=false",
                    cancellationToken);

            var sd = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            response.EnsureSuccessStatusCode();


            var dtos = JsonConvert.DeserializeObject<CoingeckoCryptoCurrencyDataDto>(sd);
            return dtos;
        }
        catch (Exception e)
        {
            //  _logger.LogError(e, "LAuto: Get details request error. Article - {Article}", article);
            return default;
        }
    }
}