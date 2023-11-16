using FakeItEasy;
using FatCat.Fakes;
using Xunit;

namespace Tests.FatCat.Toolkit.FileSystemToolsSpecs;

public class WriteAllTextTests : TestsToEnsureFileExists
{
	private readonly string textToCreate;

	public WriteAllTextTests() => textToCreate = Faker.RandomString();

	[Fact]
	public async Task WriteTextToFile()
	{
		await RunMethodToTest();

		A.CallTo(() => fileSystem.File.WriteAllTextAsync(filePath, textToCreate, default)).MustHaveHappened();
	}

	protected override async Task RunMethodToTest() { await fileTools.WriteAllText(filePath, textToCreate); }
}