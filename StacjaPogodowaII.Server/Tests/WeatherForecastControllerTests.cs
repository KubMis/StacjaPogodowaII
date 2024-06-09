using StacjaPogodowaII.Server.Controllers;
using Xunit;

namespace StacjaPogodowaII.Server.Tests
{
 
    public class WeatherForecastControllerTests
    {
        private  ILogger<WeatherForecastController> _logger;

        [Fact]
        public void ShouldVerifyIfLocationAndSummaryIsNotNull()
        {
            
            var Controler = new  WeatherForecastController(_logger);
            var location = "NewYork";
            var result = Controler.GetWeatherForecastFor7Days(location).ToArray();
            Assert.All(result, forecast =>
            {
                Assert.NotNull(forecast.Summary);
                Assert.NotNull(forecast.Location);
            });
        }

        [Fact]
        public void ShoudCheckIfDatesAreNotTheSameForWeeklyForecast()
        {

            var Controler = new WeatherForecastController(_logger);
            var location = "NewYork";
            var result = Controler.GetWeatherForecastFor7Days(location).ToArray();
           
            var dates = result.Select(forecast => forecast.Date).ToList();
            for (int i = 1; i < dates.Count; i++)
            {
                Assert.NotEqual(dates[i - 1], dates[i]);
            }
        }

        [Fact]
        public void ShoudCheckIfDatesCountIs24ForDailyForecast()
        {

            var Controler = new WeatherForecastController(_logger);
            var location = "NewYork";
            var result = Controler.GetWeatherForecastFor24Hours(location).ToArray();
           
            Assert.Equal(24, result.Length);
        }
    }
}
