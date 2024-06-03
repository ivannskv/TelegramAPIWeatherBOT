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
    public float[] visability { get; set; }
}