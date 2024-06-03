using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegram_Weather_Bot
{
    class Program
    {
        private static DateTime currentDate;
        private static Timer timer;

        static string lat = "57.7665";
        static string lon = "40.409269";
        static string url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m,wind_speed_10m&hourly=temperature_2m,relative_humidity_2m,cloud_cover,wind_speed_10m,visibility";


        private static string token { get; set; } = "7490036689:AAGZsfMTOuSareAbfoLmcfNZmWs4t7_Kh4Q";
        private static BotHost weatherBot;

        static async Task Main(string[] args)
        {
            currentDate = DateTime.Today;
            timer = new Timer(UpdateDate, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));

            weatherBot = new BotHost(token);
            weatherBot.Start();
            weatherBot.OnMessage += OnMessage;
            Console.ReadLine();
        }

        private static void UpdateDate(object? state)
        {
            DateTime today = DateTime.Today;
            if (currentDate != today)
            {
                currentDate = today;
            }
        }

        private static async void OnMessage(ITelegramBotClient client, Update update)
        {
            string command = update.Message?.Text ?? "0";
            command = command.ToLower();

            switch (command)
            {
                case "/start":
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? 654530825,
                        "Привет! Я Weather - костромской синоптик. Я буду показывать тебе погоду на текущий момент в Костроме.");
                    break;

                case "/текущая погода":
                case "текущая погода":
                    await client.SendTextMessageAsync(
                        update.Message?.Chat.Id ?? 654530825,
                        await GetWeatherInfo(url)
                        );
                    break;
            }
        }

        /// <summary>
        /// Получение информации о погоде
        /// </summary>
        /// <returns></returns>
        private static async Task<string> GetWeatherInfo(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string date = currentDate.ToString("yyyy-MM-dd");
                int h = DateTime.Now.Hour;
                Console.WriteLine(h);
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonSerializer.Deserialize<WeatherData>(jsonResponse);

                    if (weatherData != null)
                    {
                        return $"Вот какие я собрал данные о погоде на момент {date}\n" +
                            $"🌡Текущая температура воздуха: {weatherData?.current.temperature_2m}°C\n" +
                            $"🌬Скорость ветра: {weatherData?.current.wind_speed_10m}m/s\n" +
                            $"💧Влажность воздуха: {weatherData?.hourly?.relative_humidity_2m[h] ?? 0}%\n";
                    }
                    else
                        return "Данные о погоде не найдены.";
                }
                else
                    return "Не удалось получить данные о погоде. Статус: " + response.StatusCode;
            }
        }

    }
}

