using FatCat.Toolkit;
using FatCat.Toolkit.Web;
using Newtonsoft.Json;

namespace Tests.FatCat.Toolkit.Web.Api.WebCallerSpecs;

public class PutTests : WebCallerTests
{
	protected override string BasicPath
	{
		get => "/put";
	}

	protected override Task<FatWebResponse> MakeCallToWeb(string path)
	{
		return webCaller.Put(path);
	}

	public class TestData : EqualObject
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public int Number { get; set; }

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}
