using Newtonsoft.Json;

namespace LatokenTask.ExternalApis.Coingecko
{
    public class CoingeckoCryptoCurrencyDataDto
    {
        public string Symbol { get; set; }
        public string Name { get; set; }

        [JsonProperty("market_data")]
        public MarketData MarketData { get; set; }
    }

    public class MarketData
    {
        [JsonProperty("current_price")]
        public CurrentPriceUSD CurrentPrice { get; set; }
    }

    public class CurrentPriceUSD
    {
        public decimal USD { get; set; }
    }
}
