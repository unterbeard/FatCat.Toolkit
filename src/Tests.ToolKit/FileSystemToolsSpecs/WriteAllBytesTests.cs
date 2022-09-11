using FakeItEasy;
using Xunit;

namespace Tests.FatCat.Toolkit.FileSystemToolsSpecs;

public class WriteAllBytesTests : TestsToEnsureFileExists
{
	private readonly byte[] bytesToCreate;

	public WriteAllBytesTests()
	{
		bytesToCreate = new byte[]
						{
							1,
							2,
							3
						};
	}

	[Fact]
	public void WriteAllTheBytesToTheFile()
	{
		RunMethodToTest();

		A.CallTo(() => fileSystem.File.WriteAllBytesAsync(filePath, bytesToCreate, default))
		.MustHaveHappened();
	}

	protected override void RunMethodToTest() => fileTools.WriteAllBytes(filePath, bytesToCreate).Wait();
}