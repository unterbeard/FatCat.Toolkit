using FatCat.Toolkit.Console;
using FatCat.Toolkit.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OneOff;

[Authorize]
public class TestEndpoint : Endpoint
{
	[HttpGet("api/test")]
	public WebResult GetTestStuff()
	{
		ConsoleLog.WriteCyan($"In test endpoint | <{HttpContext.User.Identity.Name}>");

		return WebResult.Ok($"This is a test endpoint | <{DateTime.Now:h:mm:ss tt zz}>");
	}
}