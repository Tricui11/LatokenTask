using Telegram.Bot.Types;

namespace LatokenTask.Services.Abstract
{
    public interface ITelegramBotClient
    {
        Task SendTextMessageAsync(long chatId, string message, CancellationToken cancellationToken = default);
        void StartReceiving(Func<Update, Task> handleUpdateAsync, Func<Exception, Task> handleErrorAsync, CancellationToken cancellationToken = default);
    }
}
