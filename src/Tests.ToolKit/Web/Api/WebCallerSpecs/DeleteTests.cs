using FatCat.Toolkit.Web;

namespace Tests.FatCat.Toolkit.Web.Api.WebCallerSpecs;

public class DeleteTests : WebCallerTests
{
	protected override string BasicPath => "/delete";

	protected override async Task<WebResult> MakeCallToWeb(string path) { return await webCaller.Delete(path); }
}