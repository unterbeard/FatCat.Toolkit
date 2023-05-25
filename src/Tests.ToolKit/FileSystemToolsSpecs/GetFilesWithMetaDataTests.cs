using FakeItEasy;
using FatCat.Fakes;
using FluentAssertions;
using Xunit;

namespace Tests.FatCat.Toolkit.FileSystemToolsSpecs;

public class GetFilesWithMetaDataTests : FileToolsTests
{
	private readonly List<string> fileList;

	public GetFilesWithMetaDataTests()
	{
		fileList = Faker.Create<List<string>>();

		A.CallTo(() => fileSystem.Directory.GetFiles(directoryPath))
		.Returns(fileList.ToArray());
	}

	[Fact]
	public void IfDirectoryDoesNotExistReturnEmptyList()
	{
		directoryExists = false;

		var files = fileTools.GetFiles(directoryPath);

		files.Should()
			.BeEmpty();
	}

	[Fact]
	public void VerifyDirectoryExists()
	{
		fileTools.GetFilesWithMetaData(directoryPath);

		VerifyDirectoryExistsWasCalled();
	}

	[Fact]
	public void WillGetFiles()
	{
		fileTools.GetFilesWithMetaData(directoryPath);

		A.CallTo(() => fileSystem.Directory.GetFiles(directoryPath))
		.MustHaveHappened();
	}
}