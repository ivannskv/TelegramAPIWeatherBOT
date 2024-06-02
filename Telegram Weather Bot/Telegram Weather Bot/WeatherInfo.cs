using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace Telegram_Weather_Bot
{
    public class WeatherInfo
    {
        public float lat { get; set; }
        public float lon { get; set; }
        public string tz { get; set; }
        public string date { get; set; }
        public string units { get; set; }
        public CloudCover cloud_cover { get; set; }
        public Humidity humidity { get; set; }
        public Precipitation precipitation { get; set; }
        public Temperature temperature { get; set; }
        public Pressure pressure { get; set; }
        public Wind wind { get; set; }
    }

    public class CloudCover
    {
        public string afternoon { get; set; }
    }

    public class Humidity
    {
        public string afternoon { get; set; }
    }

    public class Precipitation
    {
        public string total { get; set; }
    }

    public class Temperature
    {
        public string min { get; set; }
        public string max { get; set; }
        public string afternoon { get; set; }
        public string night { get; set; }
        public string evening { get; set; }
        public string morning { get; set; }
    }

    public class Pressure
    {
        public string afternoon { get; set; }
    }

    public class Wind
    {
        public Max max { get; set; }
    }

    public class Max
    {
        public string speed { get; set; }
        public string direction { get; set; }
    }
}
