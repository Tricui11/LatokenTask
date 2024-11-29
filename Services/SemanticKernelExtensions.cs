using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using LatokenTask.Services.Abstract;

namespace LatokenTask.Services
{
    public static class SemanticKernelExtensions
    {
        public static IServiceCollection AddSemanticKernelServices(this IServiceCollection services, string openAiApiKey)
        {
            var builder = Kernel.CreateBuilder();

            builder.AddOpenAIChatCompletion("gpt-4-1106-preview", openAiApiKey);

            var kernel = builder.Build();

            services.AddSingleton(kernel);
            services.AddSingleton<IKernelService, KernelService>();

            return services;
        }
    }

}
