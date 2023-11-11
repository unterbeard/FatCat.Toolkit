using FakeItEasy;
using FatCat.Toolkit.Logging;
using FatCat.Toolkit.Web;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.Web.Api.WebCallerSpecs;

public class GetTests
{
	private readonly WebCaller webCaller = new(new Uri("https://httpbin.org"), A.Fake<IToolkitLogger>());

	[Fact]
	public async Task CanMakeBasicGet()
	{
		var result = await webCaller.Get("/get");

		result.IsSuccessful
			.Should()
			.BeTrue();
	}
	
	[Fact]
	public void CanMakeAGetWithQueryParameters()
	{
		true
			.Should()
			.BeFalse();
	}
}