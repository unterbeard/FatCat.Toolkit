using FatCat.Toolkit.Console;
using FatCat.Toolkit.Web;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace OneOff;

public class TestGetWithQueryStringListEndpoint : Endpoint
{
	[HttpGet("api/Test/Search/Multi")]
	public WebResult DoTestWithQueryStrings([FromQuery] List<MovieItemStatus> statuses)
	{
		ConsoleLog.WriteMagenta("Got Query Request");

		ConsoleLog.WriteMagenta(
			JsonConvert.SerializeObject(
				statuses,
				new JsonSerializerSettings
				{
					Formatting = Formatting.Indented,
					Converters = new List<JsonConverter> { new StringEnumConverter() }
				}
			)
		);

		return Ok($"Got Message | <{DateTime.Now:h:mm:ss tt zz}>");
	}
}
