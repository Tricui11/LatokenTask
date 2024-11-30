using LatokenTask.ExternalApis.Coinmarketcap;
using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

            var res = _mapper.Map<List<CryptoPriceInfo>>(responseDto.Data.SelectMany(x => x.Value));

            return res;
        }
        catch (Exception e)
        {
            //  _logger.LogError(e, "LAuto: Get details request error. Article - {Article}", article);
            return default;
        }
    }
}