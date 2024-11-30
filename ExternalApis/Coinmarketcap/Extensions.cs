using LatokenTask.Models;
using LatokenTask.Services;
using LatokenTask.Services.Abstract;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;

namespace LatokenTask.ExternalApis.Coinmarketcap;

public static class Extensions
{
    public static IServiceCollection AddCoinmarketcapApiSupport(this IServiceCollection services)
    {
        var options = services.AddValidateOptions<CoinmarketcapApiOptions>();

        services.AddKeyedScoped<IPricesService, CoinmarketcapApiService>(PriceApiServiceKeys.Coinmarketcap);

        services.AddHttpClient(HttpClientNames.Coinmarketcap,
            x =>
            {
                //   x.DefaultRequestHeaders.UserAgent.ParseAdd("LatokenTask.API");


                x.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", options.ApiKey);



                x.BaseAddress = new Uri(options.BaseUrl);
            });

        return services;
    }
}