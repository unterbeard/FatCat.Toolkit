﻿using FatCat.Toolkit.Console;
using FatCat.Toolkit.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProxySpike.Workers;

[AllowAnonymous]
public class TestUpEndpoint : Endpoint
{
	[HttpGet("api/Test")]
	public WebResult DoTestUp()
	{
		ConsoleLog.WriteMagenta("Hit the TestUp Endpoint");

		var message = $"You got me {DateTime.UtcNow:hh:mm:ss:fff tt}";

		return Ok(message);
	}
}