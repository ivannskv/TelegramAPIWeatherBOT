using System;
using System.Text.Json;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Telegram_Weather_Bot
{
    class Program
    {
        private static DateTime currentDate;
        private static Timer timer;

        static string weatherAPI = "6de2b75f56ae6a8fd6715a26f9ac2f15";
        static string city = "Kostroma";
        static string lat = "57.7708";
        static string lon = "40.9344";
        static string lang = "ru";

        private static string token { get; set; } = "7490036689:AAGZsfMTOuSareAbfoLmcfNZmWs4t7_Kh4Q";
        private static BotHost weatherBot;


        private static void Main(string[] args)
        {
            currentDate = DateTime.Today;
            timer = new Timer(UpdateDate, null, TimeSpan.Zero, TimeSpan.FromMinutes(10));


            weatherBot = new BotHost(token);
            weatherBot.Start();
            weatherBot.OnMessage += OnMessage;
            Console.ReadLine();
        }

        /// <summary>
        /// обновление даты для токена
        /// </summary>
        /// <param name="state"></param>
        private static void UpdateDate(object? state)
        {
            DateTime today = DateTime.Today;
            if(currentDate != today)
            {
                currentDate = today;
            }
        }

        /// <summary>
        /// Ответ пользователю
        /// </summary>
        /// <param name="client"></param>
        /// <param name="update"></param>
        private static async void OnMessage(ITelegramBotClient client, Update update)
        {
            string command = update.Message?.Text?? "1";
            command = command.ToLower();
            switch (command)
            {
                case "/start":
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? 654530825,
                        "Привет! Я Weather - костромской синоптик. Я буду показывать тебе погоду на текущий момент в Костроме.");
                    break;

                case "/текущая погода":
                case "текущая погода":
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? 654530825, GetWeatherInfo());
                    break;
            }
        }

        private static string GetWeatherInfo()
        {
            string message = GetRespone().GetAwaiter().GetResult();
            return message;
        }

        private static async Task<string> GetRespone()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                string date = currentDate.ToString("yyyy-MM-dd");
                string url = $"https://api.openweathermap.org/data/3.0/onecall/day_summary?lat=60.45&lon=-38.67&date=2023-03-30&tz=+03:00&appid={weatherAPI}";
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    
                }

                string jsonResponse = await response.Content.ReadAsStringAsync();
                WeatherInfo weather = JsonSerializer.Deserialize<WeatherInfo>(jsonResponse);

                if (weather != null)
                {
                    string weatherInfo = $"Погода в короде на момент {weather.date} {weather.tz}.\n" +
                        $"☁️Облачность после полудня: {weather?.cloud_cover?.afternoon}\n" +
                        $"💧Влажность после полудня: {weather?.humidity?.afternoon}%\n" +
                        $"🌧Общее кол-во осадков: {weather?.precipitation?.total} мм\n" +
                        $"🌡Температура макс/мин: {weather?.temperature?.max}/{weather?.temperature?.min}°C\n" +
                        $"Температура ☀️днем/🌙ночью: {weather?.temperature?.morning}/{weather?.temperature?.night}°C\n" +
                        $"Давление: {weather?.pressure?.afternoon} гП\n" +
                        $"🌬Скорость ветра: {weather.wind?.max.speed} м/с, направление {weather.wind?.max.direction}°";
                    return weatherInfo;
                }
            }
            
            return "не удалость получить данные о погоде";
        }
    }
}