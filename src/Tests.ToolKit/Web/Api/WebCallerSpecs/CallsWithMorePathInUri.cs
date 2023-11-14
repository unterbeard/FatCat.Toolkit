using FakeItEasy;
using FatCat.Toolkit.Json;
using FatCat.Toolkit.Logging;
using FatCat.Toolkit.Web;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Web.Api.WebCallerSpecs;

public class CallsWithMorePathInUri
{
	private const string BaseUrl = "https://httpbin.org/anything";

	private readonly WebCaller webCaller = new(new Uri(BaseUrl), new JsonOperations(), A.Fake<IToolkitLogger>());

	[Fact]
	public async Task CanMakeAGetToALongerPath()
	{
		var endingPath = "GoBengals";

		var result = await webCaller.Get(endingPath);

		result.IsSuccessful.Should().BeTrue();

		var response = result.To<HttpBinResponse>();

		var expectedFullUrl = GetExpectedUrl(endingPath);

		response.Url.Should().Be(expectedFullUrl);
	}

	private static string GetExpectedUrl(string endingPath)
	{
		return $"{BaseUrl}/{endingPath}";
	}

	[Fact]
	public void CanGetFullUrl()
	{
		var endingPath = "StarTrek";

		var expectedFullUrl = GetExpectedUrl(endingPath);

		webCaller.GetFullUrl(endingPath).Should().Be(expectedFullUrl);
	}
}
