using FatCat.Fakes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SampleDocker;

[ApiController]
[Route("api/weather")]
public class WeatherForecastController : ControllerBase
{
	private static readonly string[] Summaries =
	{
		"Freezing",
		"Bracing",
		"Chilly",
		"Cool",
		"Mild",
		"Warm",
		"Balmy",
		"Hot",
		"Sweltering",
		"Scorching"
	};

	private readonly ILogger<WeatherForecastController> _logger;

	public WeatherForecastController(ILogger<WeatherForecastController> logger) => _logger = logger;

	[HttpGet(Name = "GetWeatherForecast")]
	public IEnumerable<WeatherForecast> Get()
	{
		return Enumerable.Range(1, 5)
						.Select(index => new WeatherForecast
										{
											Date = DateTime.Now.AddDays(index),
											TemperatureC = Random.Shared.Next(-20, 55),
											Summary = Summaries[Random.Shared.Next(Summaries.Length)],
											MetaData = $"This has been added by me David Basarab - <{Faker.RandomInt()}>"
										})
						.ToArray();
	}
}