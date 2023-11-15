using FatCat.Toolkit.Console;
using FatCat.Toolkit.Web;
using FatCat.Toolkit.WebServer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProxySpike.Workers.ServiceModels;

namespace ProxySpike.Workers.Endpoints;

public class TestPostEndpoint : Endpoint
{
	[HttpPost("api/Test")]
	public WebResult DoPostTest([FromBody] SamplePostRequest request)
	{
		ConsoleLog.WriteDarkGreen($"{JsonConvert.SerializeObject(request, Formatting.Indented)}");

		return Ok($"You hit the TestPost Endpoint | {DateTime.Now:hh:mm:ss:fff tt}");
	}
}
