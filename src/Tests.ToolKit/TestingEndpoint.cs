using FatCat.Toolkit.WebServer;

namespace Tests.FatCat.Toolkit;

public class TestingEndpoint : Endpoint
{
	private readonly TestingModel modelToBeReturned;

	public TestingEndpoint(TestingModel modelToBeReturned) => this.modelToBeReturned = modelToBeReturned;

	public WebResult DoSomeWork() => Ok(modelToBeReturned);
}
