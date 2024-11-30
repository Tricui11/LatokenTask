namespace LatokenTask.Models
{
    public class CryptoPriceInfo
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public decimal PercentChange7d { get; set; }
    }
}
