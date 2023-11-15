using FatCat.Fakes;
using FatCat.Toolkit;
using FatCat.Toolkit.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OneOff;

public class TestModel : EqualObject
{
	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string SomeData { get; set; }
}

[AllowAnonymous]
public class TestEndpoint(ISomeServiceWorker someServiceWorker) : Endpoint
{
	[HttpGet("api/test")]
	public WebResult GetTestStuff()
	{
		var testModel = Faker.Create<TestModel>();

		testModel.SomeData = $"This is a test endpoint | <{DateTime.Now:h:mm:ss tt zz}>";

		someServiceWorker.DoSomeWork();

		return WebResult.Ok(testModel);
	}
}
