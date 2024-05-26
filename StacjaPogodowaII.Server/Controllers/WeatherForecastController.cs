using Microsoft.AspNetCore.Mvc;
using StacjaPogodowaII.Server.Model;

namespace StacjaPogodowaII.Server.Controllers
{
    /// <summary>
    /// Weather forecast controller to manage weather data retrieval and export.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherForecastController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets the weather forecast for the next 7 days.
        /// </summary>
        /// <param name="Location">The location for which to get the forecast.</param>
        /// <returns>An enumerable of <see cref="WeatherForecast"/> objects.</returns>
        [HttpGet("7Days/{Location}")]
        public IEnumerable<WeatherForecast> GetWeatherForecastFor7Days([FromRoute] string Location)
        {
            return Enumerable.Range(1, 7).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Location = Location,
                Humidity = Random.Shared.Next(1, 99),
                WindSpeed = Random.Shared.Next(0, 100)
            })
            .ToArray();
        }

        /// <summary>
        /// Gets the weather forecast for the next 24 hours.
        /// </summary>
        /// <param name="Location">The location for which to get the forecast.</param>
        /// <returns>An enumerable of <see cref="WeatherForecast"/> objects.</returns>
        [HttpGet("24Hours/{Location}")]
        public IEnumerable<WeatherForecast> GetWeatherForecastFor24Hours([FromRoute] string Location)
        {
            return Enumerable.Range(1, 24).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddHours(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                Location = Location,
                Humidity = Random.Shared.Next(1, 99),
                WindSpeed = Random.Shared.Next(0, 100)
            })
            .ToArray();
        }

        /// <summary>
        /// Exports the weather forecast to a CSV file.
        /// </summary>
        /// <param name="Location">The location for which to get the forecast.</param>
        /// <param name="ForecastType">The type of forecast to export ("Weekly" or "Hourly").</param>
        /// <param name="FilePath">The file path to save the CSV file.</param>
        [HttpPost("Export")]
        public void ExportForecast([FromQuery] string Location, [FromQuery] string ForecastType, [FromQuery] string FilePath)
        {
            SaveWeatherForecastToCsv(Location, ForecastType, FilePath);
        }

        /// <summary>
        /// Saves the weather forecast to a CSV file.
        /// </summary>
        /// <param name="Location">The location for which to get the forecast.</param>
        /// <param name="ForecastType">The type of forecast to export ("Weekly" or "Hourly").</param>
        /// <param name="FilePath">The file path to save the CSV file.</param>
        private void SaveWeatherForecastToCsv(string Location, string ForecastType, string FilePath)
        {
            var forecastHourly = GetWeatherForecastFor24Hours(Location);
            var forecastWeekly = GetWeatherForecastFor7Days(Location);

            switch (ForecastType)
            {
                case "Weekly":
                    using (var writer = new StreamWriter(FilePath))
                    {
                        writer.WriteLine("Date,TemperatureC,Summary,Location,Humidity,WindSpeed");

                        foreach (var forecast in forecastWeekly)
                        {
                            writer.WriteLine($"{forecast.Date},{forecast.TemperatureC},{forecast.Summary},{forecast.Location},{forecast.Humidity},{forecast.WindSpeed}");
                        }
                    }
                    break;
                case "Hourly":
                    using (var writer = new StreamWriter(FilePath))
                    {
                        writer.WriteLine("Date,TemperatureC,Summary,Location,Humidity,WindSpeed");

                        foreach (var forecast in forecastHourly)
                        {
                            writer.WriteLine($"{forecast.Date},{forecast.TemperatureC},{forecast.Summary},{forecast.Location},{forecast.Humidity},{forecast.WindSpeed}");
                        }
                    }
                    break;
                default:
                    _logger.LogInformation(ForecastType);
                    throw new ArgumentException("Wrong forecast type");
            }
        }
    }
}
