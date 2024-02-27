namespace Tests.FatCat.Toolkit.FileSystemToolsSpecs;

public class ReadAllTextTests : FileToolsTests
{
	private readonly string fileText;

	public ReadAllTextTests()
	{
		fileText = Faker.RandomString();

		A.CallTo(() => fileSystem.File.ReadAllTextAsync(A<string>._, default)).Returns(fileText);
	}

	[Fact]
	public async Task CheckIfFileExists()
	{
		await fileTools.ReadAllText(filePath);

		VerifyFileExistWasCalled();
	}

	[Fact]
	public async Task IfTheFileDoesNotExistReturnAnEmptyArray()
	{
		SetFileDoesNotExist();

		var resultingText = await fileTools.ReadAllText(filePath);

		resultingText.Should().BeEmpty();
	}

	[Fact]
	public async Task ReadAllBytesAsync()
	{
		await fileTools.ReadAllText(filePath);

		A.CallTo(() => fileSystem.File.ReadAllTextAsync(filePath, default)).MustHaveHappened();
	}

	[Fact]
	public async Task ReturnFileBytes()
	{
		var resultingText = await fileTools.ReadAllText(filePath);

		resultingText.Should().BeEquivalentTo(fileText);
	}
}
