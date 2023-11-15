using FatCat.Fakes;
using FatCat.Toolkit.Injection;
using FatCat.Toolkit.Web;
using FatCat.Toolkit.WebServer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SampleDocker;

public class GetWeatherEndpoint : Endpoint
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

	private readonly IExampleWorker exampleWorker;
	private readonly ISystemScope scope;

	public GetWeatherEndpoint(IExampleWorker exampleWorker, ISystemScope scope)
	{
		this.exampleWorker = exampleWorker;
		this.scope = scope;
	}

	[HttpGet("api/weather")]
	public WebResult GetWeather()
	{
		var secondInjectedThing = scope.Resolve<ISecondInjectedThing>();

		var items = Enumerable
			.Range(1, 5)
			.Select(
				index =>
					new WeatherForecast
					{
						Date = DateTime.Now.AddDays(index),
						TemperatureC = Random.Shared.Next(-20, 55),
						Summary = Summaries[Random.Shared.Next(Summaries.Length)],
						MetaData = $"This has been added by me David Basarab - <{Faker.RandomInt()}>",
						SecondMetaData = $"Just more fake goodness - <{Faker.RandomInt()}>",
						SomeMessage = $"{exampleWorker.GetMessage()} | <{secondInjectedThing.GetSomeNumber()}>"
					}
			)
			.ToArray();

		return WebResult.Ok(JsonConvert.SerializeObject(items));
	}
}
