using FatCat.Toolkit.Web;
using FatCat.Toolkit.WebServer;

namespace Tests.FatCat.Toolkit;

public class TestingEndpoint : Endpoint
{
	private readonly TestingModel modelToBeReturned;

	public TestingEndpoint(TestingModel modelToBeReturned)
	{
		this.modelToBeReturned = modelToBeReturned;
	}

	public WebResult DoSomeWork()
	{
		return Ok(modelToBeReturned);
	}
}
