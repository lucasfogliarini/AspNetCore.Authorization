using AspNetCore.Authorization.JsonWebToken;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Authorization.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AuthorizeApi("weather_api")]
    public class WeatherForecastController(IAuthService authorizationService, ILogger<WeatherForecastController> logger) : ControllerBase
    {
        private readonly IAuthService authorizationService = authorizationService;
        private readonly ILogger<WeatherForecastController> _logger = logger;

        [HttpGet, AuthorizeResource(nameof(Get5WeatherForecasts))]
        [Route("5")]
        public IEnumerable<WeatherForecast> Get5WeatherForecasts()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55)
            })
            .ToArray();
        }

        [HttpGet, AuthorizeResource(nameof(Get10WeatherForecasts))]
        [Route("10")]
        public IEnumerable<WeatherForecast> Get10WeatherForecasts()
        {
            return Enumerable.Range(1, 10).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55)
            })
            .ToArray();
        }
    }
}
