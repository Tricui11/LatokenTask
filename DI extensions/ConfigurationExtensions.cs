using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LatokenTask.Extensions;

public static class ConfigurationExtensions
{
    public static TModel AddValidateOptions<TModel>(this IServiceCollection service) where TModel : class, new()
    {
        service.AddOptions<TModel>()
            .BindConfiguration(typeof(TModel).Name)
            .ValidateDataAnnotations();
        var options = service.BuildServiceProvider().GetRequiredService<IOptions<TModel>>().Value;
        service.AddSingleton(options);

        return options;
    }
}
