
using System;
using Interview;
using Newtonsoft.Json;

/*
CHALLENGE 2
There's something wrong with the code being called below. Instead of taking closer to the time expected based on the Thread.Sleep() values the code it's taking much longer to return.
Your goal in this challenge is two things:
1. Determine the problem and explain why the code is taking so much longer to return than expected.
2. Fix it. Make this code run as quickly and efficiently as possible.
*/

IEnumerable<WeatherForecast> response = new ForecastService().GetNextTwoWeeks(true);
Console.WriteLine(JsonConvert.SerializeObject(response));
