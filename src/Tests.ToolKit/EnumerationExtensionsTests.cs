using FatCat.Toolkit;
using FatCat.Toolkit.Extensions;
using FatCat.Toolkit.Web.Api;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit;

public class EnumerationExtensionsTests
{
	[Fact]
	public void IsFlagSetsWorks()
	{
		var options = WebApplicationOptions.Https | WebApplicationOptions.SignalR;

		options.IsFlagSet(WebApplicationOptions.Https).Should().BeTrue();

		options.IsFlagSet(WebApplicationOptions.Authentication).Should().BeFalse();
	}
}
