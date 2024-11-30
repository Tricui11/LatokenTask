using LatokenTask.Models;
using LatokenTask.Services;
using LatokenTask.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace LatokenTask.ExternalApis.GnewsIo;

public static class Extensions
{
    public static IServiceCollection AddGnewsIoApiSupport(this IServiceCollection services)
    {
        var options = services.AddValidateOptions<GnewsIoApiOptions>();

        services.AddKeyedScoped<INewsService, GnewsIoApiService>(NewsApiServiceKeys.GnewsIo);

        services.AddHttpClient(HttpClientNames.GnewsIo,
            x =>
            {
                x.BaseAddress = new Uri(options.BaseUrl);
            });

        return services;
    }
}