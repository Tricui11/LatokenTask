using Microsoft.SemanticKernel.ChatCompletion;

namespace LatokenTask.Services.Abstract
{
    public interface IKernelService
    {
        IChatCompletionService GetChatCompletionService();
        Task<string> GetResponseFromChatAsync(ChatHistory chatHistory);
    }
}
