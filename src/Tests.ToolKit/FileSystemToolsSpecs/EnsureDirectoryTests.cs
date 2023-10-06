using FakeItEasy;
using Xunit;

namespace Tests.FatCat.Toolkit.FileSystemToolsSpecs;

public class EnsureDirectoryTests : FileToolsTests
{
	[Fact]
	public void CheckIfDirectoryExists()
	{
		fileTools.EnsureDirectory(directoryPath);

		VerifyDirectoryExistsWasCalled();
	}

	[Fact]
	public void IfDirectoryDoesNotExistCreate()
	{
		directoryExists = false;

		fileTools.EnsureDirectory(directoryPath);

		A.CallTo(() => fileSystem.Directory.CreateDirectory(directoryPath)).MustHaveHappened();
	}

	[Fact]
	public void IfDirectoryIsFoundDoNotCreate()
	{
		fileTools.EnsureDirectory(directoryPath);

		A.CallTo(() => fileSystem.Directory.CreateDirectory(A<string>._)).MustNotHaveHappened();
	}
}
