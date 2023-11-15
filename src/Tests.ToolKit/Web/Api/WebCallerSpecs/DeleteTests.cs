using FatCat.Toolkit.Web;
using FatCat.Toolkit.WebServer;

namespace Tests.FatCat.Toolkit.Web.Api.WebCallerSpecs;

public class DeleteTests : WebCallerTests
{
	protected override string BasicPath => "/delete";

	protected override async Task<FatWebResponse> MakeCallToWeb(string path)
	{
		return await webCaller.Delete(path);
	}
}
