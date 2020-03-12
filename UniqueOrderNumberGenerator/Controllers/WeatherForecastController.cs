using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UniqueOrderNumberGenerator.Service;

namespace UniqueOrderNumberGenerator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IUniqueNumberGenerator _uniqueNumberGenerator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IUniqueNumberGenerator uniqueNumberGenerator)
        {
            _logger = logger;
            _uniqueNumberGenerator = uniqueNumberGenerator;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Gets()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get([FromQuery]int id)
        {
            var number = await _uniqueNumberGenerator.NewNumberAsync();
            return Ok(number);
        }
    }
}
