namespace LatokenTask.Services.Abstract
{
    public interface IPriceService
    {
        Task<Dictionary<DateTime, decimal>> GetPriceHistoryAsync(string cryptoSymbol, DateTime startDate, DateTime endDate);
        decimal CalculatePriceChange(Dictionary<DateTime, decimal> prices);
    }
}
