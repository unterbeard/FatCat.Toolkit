using FatCat.Toolkit.Web;
using FatCat.Toolkit.WebServer;

namespace Tests.FatCat.Toolkit.Web.Api.WebCallerSpecs;

public class GetTests : WebCallerTests
{
	protected override string BasicPath => "/get";

	protected override async Task<FatWebResponse> MakeCallToWeb(string path)
	{
		return await webCaller.Get(path);
	}
}
