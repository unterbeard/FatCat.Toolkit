using FakeItEasy;
using Xunit;

namespace Tests.FatCat.Toolkit.Tools.FileSystemToolsSpecs;

public class DeleteDirectoryTests : FileToolTests
{
	[Fact]
	public void CheckIfDirectoryExists()
	{
		fileTools.DeleteDirectory(directoryPath);

		VerifyDirectoryExistsWasCalled();
	}

	[Fact]
	public void DeleteDirectoryIfFound()
	{
		fileTools.DeleteDirectory(directoryPath);

		A.CallTo(() => fileSystem.Directory.Delete(directoryPath))
		.MustHaveHappened();
	}

	[Fact]
	public void IfDirectoryIsNotFoundItIsNotDeleted()
	{
		directoryExists = false;

		fileTools.DeleteDirectory(directoryPath);

		A.CallTo(() => fileSystem.Directory.Delete(A<string>._))
		.MustNotHaveHappened();
	}
}