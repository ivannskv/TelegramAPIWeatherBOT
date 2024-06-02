using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegram_Weather_Bot
{
    class BotHost
    {
        public Action<ITelegramBotClient, Update>? OnMessage;
        private TelegramBotClient _client;

        public BotHost(string token)
        {
            _client = new TelegramBotClient(token);
        }

        public void Start()
        {
            _client.StartReceiving(UpdateHandler, ErrHandler);
            Console.WriteLine("Bot is start");
        }

        private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
        {
            var message = update.Message;
            if (message?.Text == null)
                return;
            else 
            {
                Console.WriteLine($"Chat ID: {update.Message.Chat.Id}" +
                    $"\nFirstname: {update.Message.Chat.FirstName}" +
                    $"\nUsername: {update.Message.Chat.Username}" +
                    $"\nNew message: {update.Message?.Text ?? "[IsNotTextMessage]"}\n");

                OnMessage?.Invoke(client, update);
            }
            await Task.CompletedTask;
        }

        private async Task ErrHandler(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine($"Err: " + exception.Message);
            await Task.CompletedTask;
        }
    }
}
