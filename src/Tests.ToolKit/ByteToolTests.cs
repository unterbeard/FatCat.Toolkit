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

		var textToChange = Faker.RandomString();

		var bytes = byteTools.ToBase64Encoded(textToChange);

		var plainText = byteTools.FromBase64Encoded(bytes);

		plainText
			.Should()
			.Be(textToChange);
	}
}