using LatokenTask.Models;
using LatokenTask.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;

namespace LatokenTask.Services;

public class PricesApiProvider : IPricesApiProvider
{
    private readonly IServiceProvider _serviceProvider;
   // private readonly ILogger<SupplierApiProvider> _logger;

    public PricesApiProvider(IServiceProvider serviceProvider)
       // ILogger<SupplierApiProvider> logger)
    {
        _serviceProvider = serviceProvider;
       // _logger = logger;
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
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Ошибка при получении данных от поставщика: {SupplierService}",
            //    apiService.GetType().Name);

            return default;
        }
    }

}