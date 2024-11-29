using LatokenTask.Services.Abstract;

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
            var priceHistory = new Dictionary<DateTime, decimal>
            {
                { startDate, 45000.00m },
                { startDate.AddDays(1), 46000.50m },
                { startDate.AddDays(2), 47000.75m },
                { startDate.AddDays(3), 48000.00m },
                { startDate.AddDays(4), 49000.25m },
                { startDate.AddDays(5), 50000.00m },
                { startDate.AddDays(6), 51000.50m },
                { endDate, 52000.00m }
            };

            // Вернём данные
            return await Task.FromResult(priceHistory);
        }

        public decimal CalculatePriceChange(Dictionary<DateTime, decimal> prices)
        {
            var firstPrice = prices.First().Value;
            var lastPrice = prices.Last().Value;

            return (lastPrice - firstPrice) / firstPrice * 100;
        }
    }
}
