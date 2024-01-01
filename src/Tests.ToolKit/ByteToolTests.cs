using FatCat.Fakes;
using FatCat.Toolkit;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit;

public class ByteToolTests
{
	[Fact]
	public void CanConvertToByteArrayAndFrom()
	{
		var byteTools = new ByteTools();

		var bytes = Faker.RandomBytes(124);

		var base64String = byteTools.ToBase64String(bytes);

		var decodedBytes = byteTools.FromBase64String(base64String);

		decodedBytes.Should().BeEquivalentTo(bytes);
	}
}