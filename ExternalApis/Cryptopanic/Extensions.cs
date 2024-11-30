using LatokenTask.Models;
using LatokenTask.Services;
using LatokenTask.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace LatokenTask.ExternalApis.Cryptopanic;

public static class Extensions
{
    public static IServiceCollection AddCryptopanicApiSupport(this IServiceCollection services)
    {
        var options = services.AddValidateOptions<CryptopanicApiOptions>();

        services.AddKeyedScoped<INewsService, CryptopanicApiService>(NewsApiServiceKeys.Cryptopanic);

        services.AddHttpClient(HttpClientNames.Cryptopanic,
            x =>
            {
                x.BaseAddress = new Uri(options.BaseUrl);
            });

        return services;
    }
}