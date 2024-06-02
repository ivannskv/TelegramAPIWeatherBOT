using System;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types;
using static System.Net.WebRequestMethods;

namespace Telegram_Weather_Bot
{
    class Program
    {
        private static string weatherAPI { get; set; } = "https://api.openweathermap.org/data/2.5/weather?q=Kostroma&appid=6de2b75f56ae6a8fd6715a26f9ac2f15";
        private static string token { get; set; } = "7490036689:AAGZsfMTOuSareAbfoLmcfNZmWs4t7_Kh4Q";
        private static BotHost weatherBot;
        private static HttpClientHandler httpClientHandler;
        private static HttpClient httpClent;


        private static void Main(string[] args)
        {
            httpClientHandler = new HttpClientHandler();
            httpClientHandler.UseDefaultCredentials = true;
            httpClent = new HttpClient(httpClientHandler);

            weatherBot = new BotHost(token);
            weatherBot.Start();
            weatherBot.OnMessage += OnMessage;
            Console.ReadLine();
        }

        /// <summary>
        /// Ответ пользователю
        /// </summary>
        /// <param name="client"></param>
        /// <param name="update"></param>
        private static async void OnMessage(ITelegramBotClient client, Update update)
        {
            string command = update.Message?.Text?? "1";
            switch (command)
            {
                case "/start":
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? 654530825,
                        "Привет! Я Weather - костромской синоптик. Я буду показывать тебе погоду на текущий момент в Костроме.");
                    break;

                case "/Погода":
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? 654530825, "В будущем тут будет погода");
                    break;
            }
        }
    }
}