using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace LatokenTask.Services;

public class TelegramBotClient : Abstract.ITelegramBotClient
{
    private readonly Telegram.Bot.TelegramBotClient _botClient;

    public TelegramBotClient(string token)
    {
        _botClient = new Telegram.Bot.TelegramBotClient(token);
    }

    public async Task SendTextMessageAsync(long chatId, string message, CancellationToken cancellationToken = default)
    {
        await _botClient.SendTextMessageAsync(chatId, message, cancellationToken: cancellationToken);
    }

    public void StartReceiving(Func<Update, Task> handleUpdateAsync, Func<Exception, Task> handleErrorAsync, CancellationToken cancellationToken = default)
    {
        ReceiverOptions receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>() // Принимать все типы обновлений
        };

        _botClient.StartReceiving(
            async (botClient, update, token) => await handleUpdateAsync(update),
            async (botClient, exception, token) => await handleErrorAsync(exception),
            receiverOptions,
            cancellationToken
        );

        Console.WriteLine("Bot is running...");
    }
}
