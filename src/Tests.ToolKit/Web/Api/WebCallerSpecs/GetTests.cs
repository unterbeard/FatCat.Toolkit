using FatCat.Toolkit.Web;

namespace Tests.FatCat.Toolkit.Web.Api.WebCallerSpecs;

public class GetTests : WebCallerTests
{
	protected override string BasicPath
	{
		get => "/get";
	}

	protected override async Task<FatWebResponse> MakeCallToWeb(string path) => await webCaller.Get(path);
}
