using LatokenTask.Models;
using LatokenTask.Services;
using LatokenTask.Services.Abstract;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;

namespace LatokenTask.ExternalApis.Coingecko;

public static class Extensions
{
    public static IServiceCollection AddCoingeckoApiSupport(this IServiceCollection services)
    {
        var options = services.AddValidateOptions<CoingeckoApiOptions>();

        services.AddKeyedScoped<IPricesService, CoingeckoApiService>(PriceApiServiceKeys.Coingecko);

        services.AddHttpClient(HttpClientNames.Coingecko,
            x =>
            {
                //   x.DefaultRequestHeaders.UserAgent.ParseAdd("LatokenTask.API");




                x.BaseAddress = new Uri(options.BaseUrl);
            });

        return services;
    }
}