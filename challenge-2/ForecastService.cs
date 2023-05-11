using Newtonsoft.Json;

namespace Interview
{
    public class WeatherForecast
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
        public HourlyForecast[] HourlyForecasts { get; set; }
    }

    public class HourlyForecast
    {
        public long ParentId { get; set; }
        public DateTime Time { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }

    public class ConsoleLogger
    {
        public void DebugLog(string message)
        {
            Console.WriteLine($"[Debug] {message}");
        }
    }

    public class ForecastService
    {
        private static readonly string[] Summaries = new[]
            {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ConsoleLogger _logger = new();

        public IEnumerable<WeatherForecast> GetNextTwoWeeks(bool includeHourly)
        {
            IEnumerable<WeatherForecast> dailyForecast = GetDailyForecast();
            _logger.DebugLog($"Loaded daily forecast: {JsonConvert.SerializeObject(dailyForecast)}");

            if (includeHourly)
            {
                IEnumerable<HourlyForecast> hourly = GetHourlyForecast();

                foreach (WeatherForecast forecast in dailyForecast)
                {
                    forecast.HourlyForecasts = hourly.Where(hourlyForecast => hourlyForecast.ParentId == forecast.Id).ToArray();
                }

                _logger.DebugLog($"Set hourly forecast on daily forecast: {JsonConvert.SerializeObject(hourly)}");
            }

            return dailyForecast;
        }

        IEnumerable<HourlyForecast> GetHourlyForecast()
        {
            var rng = new Random();
            return Enumerable.Range(1, 14).Select(index => Enumerable.Range(0, 23).Select(hour =>
            {
                Thread.Sleep(5);
                return new HourlyForecast
                {
                    ParentId = index,
                    Time = DateTime.Now.Date.AddDays(index).AddHours(hour),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                };
            })).SelectMany(forecast => forecast);
        }

        IEnumerable<WeatherForecast> GetDailyForecast()
        {
            var rng = new Random();
            return Enumerable.Range(1, 14).Select(index =>
            {
                Thread.Sleep(100);
                return new WeatherForecast
                {
                    Id = index,
                    Date = DateTime.Now.Date.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                };
            });
        }
    }
}
