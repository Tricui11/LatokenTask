using LatokenTask.Models;

namespace LatokenTask.Services.Abstract
{
    public interface IPricesService
    {
        Task<List<CryptoPriceInfo>> GetPricesChange7d(string cryptoSymbols, CancellationToken cancellationToken = default);
    }
}
