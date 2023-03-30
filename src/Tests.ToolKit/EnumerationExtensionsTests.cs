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
		var options = WebApplicationOptions.UseHttps | WebApplicationOptions.UseSignalR;

		options.IsFlagSet(WebApplicationOptions.UseHttps)
				.Should()
				.BeTrue();
		
		options.IsFlagSet(WebApplicationOptions.UseAuthentication)
				.Should()
				.BeFalse();
	}
}