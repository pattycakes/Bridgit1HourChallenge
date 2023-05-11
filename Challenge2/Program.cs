
using System;
using System.Collections.Generic;
using Interview;
using Newtonsoft.Json;

/*
CHALLENGE 2
There's something wrong with the code being called below. Instead of taking closer to the time expected based on the Thread.Sleep() values the code it's taking much longer to return.
Your goal in this challenge is two things:
1. Determine the problem and explain why the code is taking so much longer to return than expected.
2. Fix it. Make this code run as quickly and efficiently as possible.

1. why even have sleep in the first place? it takes much less time when you don't have it obviously. 
    but for each daily forecast you're creating an hourly forecast and waiting 5 miliseconds..
    14 * 23 * 5 , that's 1.610 , is that the expected time or did you expect it to go faster? Is it taking longer than 1.610 seconds? ( plus the 100*14 from daily ) Yes. much longer.
    It's not because of the sleep, it's the method of assigning the hourly forecasts. Each assignment of which there are 14, seems to take at least a second each. Why?  Its probably not the most efficient assignment method. 
    in fact, it's not even assigning the value to the collection. 
    If i debug and stop on line 86, I see the forcast.hourlyForecasts is assigned, but the DailyForecast is not being updated. This is reflected in the end result somehow where I am still seeing a null for hourlyForecasts. 
    I guess that's some property specifically with Ienumerables Given the collection isn't being instantiated explicitly or something. I don't know enough about IEnumerables to know why they don't work like this, 
    BUT.. a simple solution would be saying .ToList() in the foreach definition, then if you need to return an Ienumerable there's an .AsEnumerable() method as well. 
    Doing this doesn't improve performance but it does work. 

3. how would i speed it up? there's lots of options. I chose to assign it during initial creation. 
    another option which i started by creating an overloaded method is to pass in the daily forecast while creating the hourly, but i didn't continue down implementing that path. 
    This probably isn't the most efficient but I only have an hour and I don't have enough time to explore other options. This just came to mind. 

2. there's not even 24 hours in the 'hourly' forecast so that's probably a bug, it stops at 10pm -> 11pm. but doesn't include 11pm -> midnight. I decided not to fix this. 
*/

IEnumerable<WeatherForecast> response = new ForecastService().GetNextTwoWeeks(true);
Console.WriteLine(JsonConvert.SerializeObject(response));



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
            IEnumerable<WeatherForecast> dailyForecast = GetDailyForecast(includeHourly);
            _logger.DebugLog($"Loaded daily forecast: {JsonConvert.SerializeObject(dailyForecast)}");
            List<WeatherForecast> weatherForecastsList = dailyForecast.ToList();

            //if (includeHourly)
            //{
            //    IEnumerable<HourlyForecast> hourly = GetHourlyForecast();

            //    foreach (WeatherForecast forecast in weatherForecastsList)
            //    {
            //        forecast.HourlyForecasts = hourly.Where(hourlyForecast => hourlyForecast.ParentId == forecast.Id).ToArray();
            //    }

            //    _logger.DebugLog($"Set hourly forecast on daily forecast: {JsonConvert.SerializeObject(hourly)}");
            //}

            return weatherForecastsList.AsEnumerable();
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


        IEnumerable<HourlyForecast> GetOneDaysHourlyForecast(long parentId)
        {
            var rng = new Random();
            return Enumerable.Range(0, 23).Select(hour =>
            {
                Thread.Sleep(5);
                return new HourlyForecast
                {
                    ParentId = parentId,
                    Time = DateTime.Now.Date.AddDays(parentId).AddHours(hour),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                };
            });
        }

        //IEnumerable<HourlyForecast> GetHourlyForecast(IEnumerable<WeatherForecast> dailyForecast)
        //{
        //    var rng = new Random();
        //    return Enumerable.Range(1, 14).Select(index => Enumerable.Range(0, 23).Select(hour =>
        //    {
        //        Thread.Sleep(5);
        //        return new HourlyForecast
        //        {
        //            ParentId = index,
        //            Time = DateTime.Now.Date.AddDays(index).AddHours(hour),
        //            TemperatureC = rng.Next(-20, 55),
        //            Summary = Summaries[rng.Next(Summaries.Length)]
        //        };
        //    })).SelectMany(forecast => forecast);
        //}

        IEnumerable<WeatherForecast> GetDailyForecast(bool includeHourly = false)
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
                    Summary = Summaries[rng.Next(Summaries.Length)],
                    HourlyForecasts = includeHourly ? GetOneDaysHourlyForecast(index).ToArray() : null
                };
            });
        }
    }
}
