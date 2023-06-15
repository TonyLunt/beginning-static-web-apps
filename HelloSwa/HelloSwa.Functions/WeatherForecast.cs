using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace HelloSwa.Functions
{
    public static class WeatherForecast
    {
        [FunctionName("WeatherForecast")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "weather-forecast/{daysToForecast}")] HttpRequest req,
            ILogger log, int daysToForecast)
        {
           

            return new OkObjectResult(GetWeather(daysToForecast));
        }

        private static dynamic[] GetWeather(int daysToForecast)
        {
            var enumerator = Enumerable.Range(1, daysToForecast);
            var result = new List<dynamic>();
            var rnd = new Random();

            foreach (var day in enumerator)
            {
                var temp = rnd.Next(25);
                result.Add(new
                {
                    Date = DateTime.Now.AddDays(day),
                    TemperatureC = temp,
                    Summary = GetSummary(temp)
                }) ;
            }
            return result.ToArray();
        }

        private static object GetSummary(int temp)
        {
            return temp switch
            {
                int i when (i > 20) => "Hot",
                int i when (i > 10) => "Warm",
                int i when (i > 0) => "Cold",
                int i when (i > -10) => "Freezing",
                _ => "Too cold to live"
            };
        }
    }
}
