using LatokenTask.ExternalApis;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace LatokenTask.Mapping;

public static class Extensions
{
    public static IServiceCollection AddCustomMapster(this IServiceCollection services)
    {
        var mapsterConfig = new TypeAdapterConfig();
        var mapsterRegister = new MapsterRegister();
        mapsterRegister.Register(mapsterConfig);
        services.AddSingleton(mapsterConfig);
        services.AddScoped<IMapper, Mapper>();

        return services;
    }
}
