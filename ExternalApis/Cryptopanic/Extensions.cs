using System.Net.Http.Headers;
using System.Text;
using LatokenTask.ExternalApis.Cryptopanic;
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

        //services.AddKeyedScoped<INewsService, CryptopanicApiService>(NewsApiServiceKeys.Cryptopanic);

        //services.AddHttpClient(HttpClientNames.Cryptopanic,
        //    x =>
        //    {
        //        var credBytes = Encoding.UTF8.GetBytes($"{options.Login}:{options.Password}");
        //        var credsBase64 = Convert.ToBase64String(credBytes);

        //        x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credsBase64);
        //        x.BaseAddress = new Uri("https://www.goperigon.com/account/api-key");
        //    });

        return services;
    }
}