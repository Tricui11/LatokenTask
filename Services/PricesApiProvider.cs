using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace LatokenTask.Services;

public class PricesApiProvider : IPricesApiProvider
{
    private readonly IServiceProvider _serviceProvider;

    public PricesApiProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<List<CryptoPriceInfo>> GetPricesChange7d(PriceApiServiceKeys serviceKey,
    string cryptoSymbol, CancellationToken cancellationToken = default)
    {
        try
        {
            var apiService = _serviceProvider.GetKeyedService<IPricesService>(serviceKey);

            var Prices = await apiService.GetPricesChange7d(cryptoSymbol, cancellationToken);

            return Prices;
        }
        catch
        {
            return new List<CryptoPriceInfo>();
        }
    }

}