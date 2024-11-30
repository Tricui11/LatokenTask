using LatokenTask.Models;

namespace LatokenTask.Services.Abstract
{
    public interface IPricesApiProvider
    {
        Task<List<CryptoPriceInfo>> GetPricesChange7d(PriceApiServiceKeys serviceKey,
            string cryptoSymbol, CancellationToken cancellationToken = default);
    }
}
