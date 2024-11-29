using LatokenTask.Models;
using LatokenTask.Services;
using LatokenTask.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace LatokenTask.ExternalApis.NewsapiOrg;

public static class Extensions
{
    public static IServiceCollection AddNewsapiOrgApiSupport(this IServiceCollection services)
    {
        var options = services.AddValidateOptions<NewsapiOrgApiOptions>();

        services.AddKeyedScoped<INewsService, NewsapiOrgApiService>(NewsApiServiceKeys.NewsapiOrg);

        services.AddHttpClient(HttpClientNames.NewsapiOrg,
            x =>
            {
                x.DefaultRequestHeaders.UserAgent.ParseAdd("LatokenTask.API");
                x.BaseAddress = new Uri(options.BaseUrl);
            });

        return services;
    }
}