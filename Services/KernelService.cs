using LatokenTask.Services.Abstract;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel;

namespace LatokenTask.Services
{
    public class KernelService : IKernelService
    {
        private readonly Kernel _kernel;

        public KernelService(Kernel kernel)
        {
            _kernel = kernel;
        }

        public IChatCompletionService GetChatCompletionService()
        {
            return _kernel.GetRequiredService<IChatCompletionService>();
        }

        public async Task<string> GetResponseFromChatAsync(ChatHistory chatHistory)
        {
            var chatCompletionService = GetChatCompletionService();

            var result = chatCompletionService.GetStreamingChatMessageContentsAsync(
                chatHistory,
                new OpenAIPromptExecutionSettings { Temperature = 0 },
                kernel: _kernel
            );

            string fullMessage = "";

            await foreach (var content in result)
            {
                fullMessage += content.Content;
            }

            return fullMessage;
        }
    }
}
