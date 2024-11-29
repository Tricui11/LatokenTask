using LatokenTask.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LatokenTask.Services
{
    public class PriceService : IPriceService
    {
        private readonly HttpClient _httpClient;

        public PriceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Dictionary<DateTime, decimal>> GetPriceHistoryAsync(string cryptoSymbol, DateTime startDate, DateTime endDate)
        {
            //string url = $"https://api.coingecko.com/api/v3/coins/{cryptoSymbol}/market_chart/range?vs_currency=usd&from={startDate.ToUnixTime()}&to={endDate.ToUnixTime()}";
            //var response = await _httpClient.GetStringAsync(url);
            //var priceData = JsonConvert.DeserializeObject<CoinGeckoPriceResponse>(response);

            //return priceData.Prices.ToDictionary(p => DateTimeOffset.FromUnixTimeMilliseconds((long)p[0]).DateTime, p => p[1]);
            return null;
        }

        public decimal CalculatePriceChange(Dictionary<DateTime, decimal> prices)
        {
            var firstPrice = prices.First().Value;
            var lastPrice = prices.Last().Value;

            return ((lastPrice - firstPrice) / firstPrice) * 100;
        }
    }
}
