using LatokenTask.ExternalApis.Coinmarketcap;
using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Runtime.Intrinsics.X86;
using System.Threading;

namespace LatokenTask.Services;

public class CoinmarketcapApiService : IPricesService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CoinmarketcapApiService> _logger;

    public CoinmarketcapApiService(IHttpClientFactory httpClientFactory,
        ILogger<CoinmarketcapApiService> logger)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient(HttpClientNames.Coinmarketcap);
    }

    // Метод для получения процентного изменения за 7 дней из данных CoinMarketCap
    public async Task<List<CryptoPriceInfo>> GetPricesChange7d(string cryptoSymbols, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient
                .GetAsync($"cryptocurrency/quotes/latest?symbol={cryptoSymbols}",
                    cancellationToken);

            var sd = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            response.EnsureSuccessStatusCode();


            var responseDto = JsonConvert.DeserializeObject<CoinMarketCapResponseDto>(sd);

            var res = responseDto.Data.SelectMany(x => x.Value).Select(c => new CryptoPriceInfo
            {
                Name = c.Name,
                Symbol = c.Symbol,
                Price = c.Quote.USD.Price,
                PercentChange7d = c.Quote.USD.PercentChange7d,
                Source = "Coinmarketcap"
            }).ToList();

            return res;
        }
        catch (Exception e)
        {
            //  _logger.LogError(e, "LAuto: Get details request error. Article - {Article}", article);
            return default;
        }
    }
}