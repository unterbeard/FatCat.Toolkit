using FakeItEasy;
using FatCat.Fakes;
using Xunit;

namespace Tests.FatCat.Toolkit.Tools.FileSystemToolsSpecs;

public class WriteAllTextTests : TestsToEnsureFileExists
{
	private readonly string textToCreate;

	public WriteAllTextTests() => textToCreate = Faker.RandomString();

	[Fact]
	public void WriteTextToFile()
	{
		RunMethodToTest();

		A.CallTo(() => fileSystem.File.WriteAllTextAsync(filePath, textToCreate, default))
		.MustHaveHappened();
	}

	protected override void RunMethodToTest() => fileTools.WriteAllText(filePath, textToCreate).Wait();
}