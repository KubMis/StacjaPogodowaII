namespace StacjaPogodowaII.Server.Model
{
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public int Humidity { get; set; }

        public Double WindSpeed { get; set; }

        public string? Summary { get; set; }

        public string? Location { get; set; }
    }
}
