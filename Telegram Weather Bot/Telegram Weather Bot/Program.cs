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

        static string lat = "57.7708";
        static string lon = "40.9344";

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
            currentDate = DateTime.Today;
        }

        private static async void OnMessage(ITelegramBotClient client, Update update)
        {
            string command = update.Message?.Text ?? "1";
            command = command.ToLower();
            switch (command)
            {
                case "/start":
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? 654530825,
                        "Привет! Я Weather - костромской синоптик. Я буду показывать тебе погоду на текущий момент в Костроме.");
                    break;

                case "/текущая погода":
                case "текущая погода":
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? 654530825, await GetWeatherInfo());
                    break;
            }
        }

        private static async Task<string> GetWeatherInfo()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m,wind_speed_10m&hourly=temperature_2m,relative_humidity_2m,wind_speed_10m";
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var weatherData = JsonSerializer.Deserialize<WeatherData>(jsonResponse);

                    if (weatherData != null)
                    {
                        // Извлекаем данные о текущей погоде
                        var currentWeather = weatherData.current;
                        var currentTemperature = currentWeather.temperature_2m;
                        var currentWindSpeed = currentWeather.wind_speed_10m;

                        // Извлекаем данные о погоде на текущий час
                        var hourlyWeather = weatherData.hourly;
                        var hourlyTemperature = hourlyWeather.temperature_2m[0]; // Первый час
                        var hourlyRelativeHumidity = hourlyWeather.relative_humidity_2m[0];
                        var hourlyWindSpeed = hourlyWeather.wind_speed_10m[0];

                        // Формируем сообщение о погоде
                        return $"Погода в городе на момент {DateTime.Now}:\n" +
                               $"💧 Влажность после полудня: {hourlyRelativeHumidity}%\n" +
                               $"🌡 Температура макс/мин: {currentTemperature}°C\n" +
                               $"Температура ☀️ днем/🌙 ночью: {hourlyTemperature}°C\n" +
                               $"🌬 Скорость ветра: {currentWindSpeed} м/с";
                    }
                    else
                    {
                        return "Данные о погоде не найдены.";
                    }
                }
                else
                {
                    return "Не удалось получить данные о погоде. Статус: " + response.StatusCode;
                }
            }
        }

    }

    public class WeatherData
    {
        public CurrentWeather current { get; set; }
        public HourlyWeather hourly { get; set; }
    }

    public class CurrentWeather
    {
        public float temperature_2m { get; set; }
        public float wind_speed_10m { get; set; }
    }

    public class HourlyWeather
    {
        public float[] temperature_2m { get; set; }
        public int[] relative_humidity_2m { get; set; }
        public float[] wind_speed_10m { get; set; }
    }
}

