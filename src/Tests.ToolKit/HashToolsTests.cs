using FatCat.Toolkit;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit;

public class HashToolsTests
{
	[Fact]
	public void AnyChangesCausesHashEqualsToBeFalse()
	{
		var firstHash = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

		var secondHash = new byte[] { 1, 2, 3, 4, 5, 5, 7, 8, 9, 10 };

		var hashTools = new HashTools();

		hashTools.HashEquals(firstHash, secondHash).Should().BeFalse();
	}

	[Fact]
	public void SameHashesWillEqual()
	{
		var firstHash = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

		var secondHash = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

		var hashTools = new HashTools();

		hashTools.HashEquals(firstHash, secondHash).Should().BeTrue();
	}
}
