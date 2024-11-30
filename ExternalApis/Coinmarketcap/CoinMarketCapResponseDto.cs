using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace LatokenTask.ExternalApis.Coinmarketcap
{
    public class USD
    {
        public decimal Price { get; set; }
        [JsonProperty("percent_change_7d")]
        public decimal PercentChange7d { get; set; }
    }
    public class CryptoQuote
    {
        public USD USD { get; set; }
    }

    public class CryptoData
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public CryptoQuote Quote { get; set; }
    }

    public class CoinMarketCapResponseDto
    {
        public Dictionary<string, List<CryptoData>> Data { get; set; }
    }
}
