using FatCat.Toolkit;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit;

public class GeneratorTests
{
	[Fact]
	public void ByteArrayLengthIsCorrect()
	{
		var generator = new Generator();

		var bytes = generator.Bytes(1024);

		bytes.Length
			.Should()
			.Be(1024);
	}
}