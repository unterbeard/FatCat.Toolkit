using FatCat.Toolkit.Console;
using FatCat.Toolkit.Web;
using Microsoft.AspNetCore.Mvc;

namespace OneOff;

public class TestEndpoint : Endpoint
{
	[HttpGet("api/test")]
	public WebResult GetTestStuff()
	{
		ConsoleLog.WriteCyan("In test endpoint");

		return WebResult.Ok($"This is a test endpoint | <{DateTime.Now:h:mm:ss tt zz}>");
	}
}